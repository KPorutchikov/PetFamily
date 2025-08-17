using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Contracts;
using PetFamily.Species.Presentation;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.Models;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Volunteers.AddPet;

public class AddPetHandler : ICommandHandler<Guid,AddPetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;

    private readonly ISpeciesContract _speciesContract;
    private readonly IBreedContract _breedContract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        IVolunteerRepository volunteerRepository,
        ISpeciesContract speciesContract,
        IBreedContract breedContract,
        [FromKeyedServices(Modules.Volunteers)]IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> logger, 
        IValidator<AddPetCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _speciesContract = speciesContract;
        _breedContract = breedContract;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken ct = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, ct);
        if (validatorResult.IsValid == false)
            return validatorResult.ToErrorList();
            
        try
        {
            var volunteerResult = await _volunteerRepository.GetById(VolunteerId.Create(command.VolunteerId), ct);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var speciesExist = _speciesContract.GetSpecies(command.SpeciesId, ct);
            if (speciesExist.Result.TotalCount == 0)
                return Errors.General.NotFound(command.SpeciesId).ToErrorList();
            
            var breedExist = _breedContract.GetBreed(command.BreedId, ct);
            if (breedExist.Result.TotalCount == 0)
                return Errors.General.NotFound(command.BreedId).ToErrorList();
            
            var breed = PetBreed.Create(command.SpeciesId, command.BreedId).Value;
            var address = Address.Create(command.City, command.Street, command.HouseNumber, command.ApartmentNumber).Value;
            var status = PetStatus.Create((PetStatus.Status)command.Status).Value;

            var pet = Pet.Create(PetId.NewId(), command.Name, breed, command.Description, command.Color,
                command.Height, command.Weight, command.HealthInformation, address, command.Phone,
                command.IsCastrated, command.BirthDate, command.IsVaccinated, status).Value;

            volunteerResult.Value.AddPet(pet);

            await _unitOfWork.SaveChanges(ct);
            
            return pet.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Can not add pet to volunteer - {id} in transaction", command.VolunteerId);

            return Error.Failure("Can not add pet to volunteer", "volunteer.pet.failure").ToErrorList();
        }
    }
}