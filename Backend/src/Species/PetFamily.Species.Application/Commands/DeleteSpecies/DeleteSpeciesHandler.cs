using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Species.Application.Commands.DeleteSpecies;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteSpeciesHandler> _logger;
    private readonly IVolunteersContract _volunteersContract;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        ILogger<DeleteSpeciesHandler> logger,
        IVolunteersContract volunteersContract,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _volunteersContract = volunteersContract;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(DeleteSpeciesCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var queryPets = new PetExistsRequest(null, null, null, null, command.SpeciesId, null, null, null, null);
            var petsExists = _volunteersContract.GetPet(queryPets, cancellationToken);
            if (petsExists.Result.TotalCount != 0)
                return Errors.General.ValueIsInvalid("There's pet(s) with this type of species. "+command.SpeciesId.ToString()).ToErrorList();
            
            var species = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
            if (species.IsFailure)
                return species.Error.ToErrorList();

            _speciesRepository.DeleteSpecies(species.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Species was deleted - {sql}", command.SpeciesId);
            
            return command.SpeciesId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Species was not deleted - {id} ", command.SpeciesId);

            return Error.Failure("Can not delete species", "species.failure").ToErrorList();
        }
    }
}