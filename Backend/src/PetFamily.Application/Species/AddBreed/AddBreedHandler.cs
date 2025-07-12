using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesManagement.Queries.GetBreedsDapper;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Application.Species.AddBreed;

public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly GetBreedHandlerDapper _getBreedHandlerDapper;
    private readonly ILogger<AddBreedHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddBreedCommand> _validator;

    public AddBreedHandler(
        ISpeciesRepository speciesRepository,
        GetBreedHandlerDapper getBreedHandlerDapper,
        ILogger<AddBreedHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<AddBreedCommand> validator)
    {
        _speciesRepository = speciesRepository;
        _getBreedHandlerDapper = getBreedHandlerDapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddBreedCommand command, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var breedExist = _getBreedHandlerDapper.Handle(new GetBreedQuery(null, command.SpeciesId, command.Name), ct);
        if (breedExist.Result.TotalCount > 0)
        {
            _logger.LogError("Failed to create. Breed is exists: {name}", command.Name);
            return Errors.Breed.AlreadyExist().ToErrorList();
        }

        var breed = Domain.Specieses.Breed.Create(BreedId.NewId(), command.Name!).Value;

        var species = _speciesRepository.GetById(command.SpeciesId!, ct).Result.Value;
        if (species == null)
        {
            _logger.LogError("Failed to create. Species is not exists: {id}", command.SpeciesId);
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();
        }
        
        species.AddBreed(breed);
        
        await _unitOfWork.SaveChanges(ct);

        _logger.LogInformation("Created breed [{name}] with [{@id}]", command.Name, breed.Id);

        return breed.Id.Value;
    }
}