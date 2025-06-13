using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.EditPet.DeletePet;

public class DeletePetSoftHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeletePetSoftHandler> _logger;
    private readonly IValidator<DeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePetSoftHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetSoftHandler> logger, IValidator<DeletePetCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePetCommand command,
        CancellationToken ct = default)
    {
        var validationResult= await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var pet = await _volunteerRepository.GetPetById(command.PetId, ct);
        if (pet.IsFailure)
            return pet.Error.ToErrorList();

        pet.Value.SoftDelete();
        
        await _unitOfWork.SaveChanges(ct);
        
        _logger.LogInformation("Pet was deleted (soft) with id: {Id}.", command.PetId);

        return command.PetId;
    }
}