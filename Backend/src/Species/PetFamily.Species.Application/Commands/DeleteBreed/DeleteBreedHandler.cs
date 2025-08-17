using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Species.Application.Commands.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteBreedHandler> _logger;
    private readonly IVolunteersContract _volunteersContract;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBreedHandler(
        ISpeciesRepository speciesRepository,
        ILogger<DeleteBreedHandler> logger,
        IVolunteersContract volunteersContract,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _volunteersContract = volunteersContract;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var queryPets = new PetExistsRequest(null, null, null, null, null, command.BreedId, null, null, null);
            var petsExists = _volunteersContract.GetPet(queryPets, cancellationToken);
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