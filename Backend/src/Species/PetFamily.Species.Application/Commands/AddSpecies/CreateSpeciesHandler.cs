using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Species.Application.Commands.AddSpecies;

public class CreateSpeciesHandler : ICommandHandler<Guid, AddSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpeciesHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddSpeciesCommand> _validator;

    public CreateSpeciesHandler(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesHandler> logger,
        IValidator<AddSpeciesCommand> validator,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddSpeciesCommand command, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var speciesResult = await _speciesRepository.GetByFullName(command.Name, ct);

        if (speciesResult.IsSuccess)
        {
            _logger.LogError("Failed to create. Species is exists: {name}", command.Name);
            return Errors.Species.AlreadyExist().ToErrorList();
        }

        var species = Domain.Models.Species.Create(SpeciesId.NewId(), command.Name, command.Title).Value;

        await _speciesRepository.Add(species, ct);
        await _unitOfWork.SaveChanges(ct);

        _logger.LogInformation("Created species [{name}] with [{@id}]", command.Name, species.Id);

        return (Guid)species.Id;
    }
}