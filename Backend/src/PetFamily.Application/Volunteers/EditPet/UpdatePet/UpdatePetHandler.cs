using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;
using static PetFamily.Domain.Volunteers.ValueObjects.PetStatus;

namespace PetFamily.Application.Volunteers.EditPet.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid,UpdatePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<UpdatePetCommand> _validator;
    private readonly ILogger<UpdatePetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePetHandler(
        IVolunteerRepository volunteerRepository,
        ISpeciesRepository speciesRepository,
        IValidator<UpdatePetCommand> validator,
        ILogger<UpdatePetHandler> logger, 
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdatePetCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult= await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var breed = await _speciesRepository.GetBreedByBreedId(command.BreedId, cancellationToken);
        if (breed.IsFailure)
            return breed.Error.ToErrorList();

        var species = await _speciesRepository.GetSpeciesByBreedId(command.BreedId, cancellationToken);
        if (species.IsFailure)
            return species.Error.ToErrorList();
        
        var pet = await _volunteerRepository.GetPetById(command.PetId, cancellationToken);
        if (pet.IsFailure)
            return pet.Error.ToErrorList();
        
        pet.Value.UpdateMain(command.Name, PetBreed.Create(command.SpeciesId, command.BreedId).Value, command.Description,
            command.Color, command.Weight, command.Height, command.HealthInformation,
            Address.Create(command.City, command.Street, command.HouseNumber, command.ApartmentNumber).Value,
            command.Phone, command.IsCastrated, command.BirthDate, command.IsVaccinated,
            PetStatus.Create((Status)command.Status).Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet was successfully updated.");
        return command.PetId;
    }
}