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
        
        var petCurrent = volunteer.Value.Pets.Where(p => p.Id == command.PetId)!.FirstOrDefault();

        var result = volunteer.Value.MovePet(petCurrent!, SerialNumber.Create(command.SerialNumber).Value);
        if (result.IsFailure)
            return result.Error;

        await _unitOfWork.SaveChanges(ct);
        
        _logger.LogInformation("Move pet {@pet} in volunteer [{@volunteer}]", petCurrent, volunteer.Value);

        return command.PetId;
    }
}