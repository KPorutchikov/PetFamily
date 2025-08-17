using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.Core.Models;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteersContract
{
    Task<PagedList<VolunteerDto>> GetVolunteer(VolunteerExistsRequest req, CancellationToken cancellationToken);
    
    Task<PagedList<PetDto>> GetPet(PetExistsRequest req, CancellationToken cancellationToken);
}