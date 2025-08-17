using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Volunteers.Application.Volunteers.AddPet;

namespace PetFamily.Volunteers.Application.Volunteers.UpdatePetMainPhoto;

public class UpdatePetMainPhotoHandler : ICommandHandler<Guid,UpdatePetMainPhotoCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePetMainPhotoCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;

    public UpdatePetMainPhotoHandler(
        IVolunteerRepository volunteerRepository, 
        [FromKeyedServices(Modules.Volunteers)]IUnitOfWork unitOfWork,
        IValidator<UpdatePetMainPhotoCommand> validator,
        ILogger<AddPetHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdatePetMainPhotoCommand command, CancellationToken cancellationToken)
    {
        var validationResult= await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var pet = await _volunteerRepository.GetPetById(command.PetId, cancellationToken);
        if (pet.IsFailure)
            return pet.Error.ToErrorList();

        if (!string.IsNullOrWhiteSpace(command.PathToFile))
        {
            var photoExist = pet.Value.Files!.Values.Any(f => f.PathToStorage.Path == command.PathToFile);
            if (!photoExist)
                return pet.Error.ToErrorList();
        }

        pet.Value.UpdatePetPhoto(command.PathToFile!);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("PetPhoto was successfully updated.");
        
        return command.PetId;
    }
}