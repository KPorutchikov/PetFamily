using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.EditPet.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<Guid,UpdatePetStatusCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<UpdatePetStatusCommand> _validator;
    private readonly ILogger<UpdatePetStatusHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePetStatusHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<UpdatePetStatusCommand> validator,
        ILogger<UpdatePetStatusHandler> logger, 
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdatePetStatusCommand command, CancellationToken cancellationToken)
    {
        var validationResult= await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var pet = await _volunteerRepository.GetPetById(command.PetId, cancellationToken);
        if (pet.IsFailure)
            return pet.Error.ToErrorList();
        
        pet.Value.SetStatus(PetStatus.Create((PetStatus.Status)command.Status).Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("PetStatus was successfully updated.");
        
        return command.PetId;
    }
}