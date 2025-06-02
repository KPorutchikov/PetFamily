using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteBreedHandler> _logger;
    private readonly GetPetsWithPaginationHandlerDapper _getPetsWithPaginationHandlerDapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBreedHandler(
        ISpeciesRepository speciesRepository,
        ILogger<DeleteBreedHandler> logger,
        GetPetsWithPaginationHandlerDapper getPetsWithPaginationHandlerDapper,
        IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _getPetsWithPaginationHandlerDapper = getPetsWithPaginationHandlerDapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var queryPets = new GetPetsWithPaginationQuery(null,null,null,null,command.BreedId,null,null,1,100);
            var petsExists = _getPetsWithPaginationHandlerDapper.Handle(queryPets, cancellationToken);
            if (petsExists.Result.TotalCount != 0)
                return Errors.General.ValueIsInvalid("There's pet(s) with this type of breed. "+command.BreedId.ToString()).ToErrorList();
            
            var breed = await _speciesRepository.GetBreedByBreedId(command.BreedId, cancellationToken);
            if (breed.IsFailure)
                return breed.Error.ToErrorList();

            _speciesRepository.DeleteBreed(breed.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Breed was deleted - {sql}", command.BreedId);
            
            return command.BreedId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Breed was not deleted - {id} ", command.BreedId);

            return Error.Failure("Can not delete breed", "species.breed.failure").ToErrorList();
        }
    }
}