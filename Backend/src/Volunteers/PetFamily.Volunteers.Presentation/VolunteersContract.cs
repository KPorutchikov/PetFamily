using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.Core.Models;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetPetsWithPagination;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Contracts;
using PetFamily.Volunteers.Contracts.Requests;
using PetFamily.Volunteers.Presentation.Pets.Requests;

namespace PetFamily.Volunteers.Presentation;

public class VolunteersContract(
    GetVolunteersWithPaginationHandlerDapper getVolunteers,
    GetPetsWithPaginationHandlerDapper getPets) : IVolunteersContract
{
    public async Task<PagedList<VolunteerDto>> GetVolunteer(VolunteerExistsRequest req, CancellationToken cancellationToken)
    {
        return await getVolunteers.Handle(new GetVolunteersWithPaginationQuery(req.volunteerId.ToString(),
            req.FullName, req.Email, null, null, null, null), cancellationToken);
    }

    public async Task<PagedList<PetDto>> GetPet(PetExistsRequest req, CancellationToken cancellationToken)
    {
        return await getPets.Handle(new GetPetsWithPaginationQuery(req.PetId, req.VolunteerId, req.Name, req.Description,
            req.SpeciesId, req.BreedId, req.Color, req.AddressCity, req.AddressStreet, null, null, null), cancellationToken);
    }
}
