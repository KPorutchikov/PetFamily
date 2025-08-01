﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesManagement.Queries.GetBreedsDapper;
using PetFamily.Application.SpeciesManagement.Queries.GetSpeciesDapper;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler : ICommandHandler<Guid,AddPetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly GetSpeciesHandlerDapper _getSpeciesHandlerDapper;
    private readonly GetBreedHandlerDapper _getBreedHandlerDapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        IVolunteerRepository volunteerRepository,
        GetSpeciesHandlerDapper getSpeciesHandlerDapper,
        GetBreedHandlerDapper getBreedHandlerDapper,
        IUnitOfWork unitOfWork,
        ILogger<AddPetHandler> logger, 
        IValidator<AddPetCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _getSpeciesHandlerDapper = getSpeciesHandlerDapper;
        _getBreedHandlerDapper = getBreedHandlerDapper;
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

            var speciesExist = _getSpeciesHandlerDapper.Handle(new GetSpeciesQuery(command.SpeciesId), ct);
            if (speciesExist.Result.TotalCount == 0)
                return Errors.General.NotFound(command.SpeciesId).ToErrorList();
            
            var breedExist = _getBreedHandlerDapper.Handle(new GetBreedQuery(command.BreedId, command.SpeciesId, null), ct);
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