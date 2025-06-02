using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.DeleteSpecies;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteSpeciesHandler> _logger;
    private readonly GetPetsWithPaginationHandlerDapper _getPetsWithPaginationHandlerDapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpeciesHandler(
        ISpeciesRepository speciesRepository,
        ILogger<DeleteSpeciesHandler> logger,
        GetPetsWithPaginationHandlerDapper getPetsWithPaginationHandlerDapper,
        IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _getPetsWithPaginationHandlerDapper = getPetsWithPaginationHandlerDapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(DeleteSpeciesCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var queryPets = new GetPetsWithPaginationQuery(null,null,null,command.SpeciesId,null,null,null,1,100);
            var petsExists = _getPetsWithPaginationHandlerDapper.Handle(queryPets, cancellationToken);
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