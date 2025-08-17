using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Domain;
using PetFamily.Species.Domain.Models;

namespace PetFamily.Species.Application.Commands.AddBreed;

public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<AddBreedHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddBreedCommand> _validator;

    public AddBreedHandler(
        ISpeciesRepository speciesRepository,
        ILogger<AddBreedHandler> logger,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork,
        IValidator<AddBreedCommand> validator)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddBreedCommand command, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var breed = Breed.Create(BreedId.NewId(), command.Name!).Value;

        var species = _speciesRepository.GetById(command.SpeciesId!, ct).Result.Value;
        if (species == null)
        {
            _logger.LogError("Failed to create. Species is not exists: {id}", command.SpeciesId);
            return Errors.General.NotFound(command.SpeciesId).ToErrorList();
        }

        if (species.Breeds.Any(b => b.Name == command.Name))
        {
            _logger.LogError("Failed to create. Breed is exists: {name}", command.Name);
            return Errors.Breed.AlreadyExist().ToErrorList();
        }

        species.AddBreed(breed);

        await _unitOfWork.SaveChanges(ct);

        _logger.LogInformation("Created breed [{name}] with [{@id}]", command.Name, breed.Id);

        return breed.Id.Value;
    }
}