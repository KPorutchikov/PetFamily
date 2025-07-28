using PetFamily.Application.VolunteerManagement.Queries.GetPet;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetPetRequest(Guid PetId)
{
    public GetPetQuery ToQuery() => new GetPetQuery(PetId);
}