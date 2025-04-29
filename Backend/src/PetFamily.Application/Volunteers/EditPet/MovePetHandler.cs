using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;

namespace PetFamily.Application.Volunteers.EditPet;

public class MovePetHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;

    public MovePetHandler(
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(MovePetCommand command, CancellationToken ct = default)
    {
        var volunteer = await _volunteerRepository.GetByPetId(command.PetId, ct);
        if (volunteer.IsFailure)
            return volunteer.Error;

        if (command.SerialNumber <= 0 || command.SerialNumber > volunteer.Value.Pets.Count)
            return Errors.General.ValueIsInvalid("SerialNumber");

        var petCurrent = volunteer.Value.Pets.Where(p => p.Id == command.PetId)!.FirstOrDefault();

        var incriment = petCurrent!.SerialNumber.Value > command.SerialNumber ? -1 : 1;

        var transaction = await _unitOfWork.BeginTransaction(ct);
        try
        {
            while (petCurrent.SerialNumber.Value != command.SerialNumber)
            {
                volunteer.Value.Pets
                    .FirstOrDefault(p => p.SerialNumber.Value == petCurrent.SerialNumber.Value + incriment)!
                    .SetSerialNumber(SerialNumber.Create(petCurrent.SerialNumber.Value).Value);

                petCurrent.SetSerialNumber(SerialNumber.Create(petCurrent.SerialNumber.Value + incriment).Value);
            }

            await _unitOfWork.SaveChanges(ct);

            transaction.Commit();

            return command.PetId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Can not move pet in volunteer - {id} in transaction", command.PetId);

            transaction.Rollback();

            return Error.Failure("Can not move pet in volunteer", "volunteer.pet.failure");
        }
    }
}