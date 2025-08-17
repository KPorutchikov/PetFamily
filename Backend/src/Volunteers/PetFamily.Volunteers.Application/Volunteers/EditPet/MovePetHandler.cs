using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers.AddPet;

namespace PetFamily.Volunteers.Application.Volunteers.EditPet;

public class MovePetHandler : ICommandHandler<Guid,MovePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<MovePetCommand> _validator;

    public MovePetHandler(
        IVolunteerRepository volunteerRepository,
        [FromKeyedServices(Modules.Volunteers)]IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> logger, 
        IValidator<MovePetCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(MovePetCommand command, CancellationToken ct = default)
    {
        var validationResult= await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var volunteer = await _volunteerRepository.GetByPetId(command.PetId, ct);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();
        
        var petCurrent = volunteer.Value.Pets.Where(p => p.Id == command.PetId)!.FirstOrDefault();

        var result = volunteer.Value.MovePet(petCurrent!, SerialNumber.Create(command.SerialNumber).Value);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await _unitOfWork.SaveChanges(ct);
        
        _logger.LogInformation("Move pet {@pet} in volunteer [{@volunteer}]", petCurrent, volunteer.Value);

        return command.PetId;
    }
}