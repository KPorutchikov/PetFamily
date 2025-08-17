using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetPet;

namespace PetFamily.Volunteers.Presentation.Pets.Requests;

public record GetPetRequest(Guid PetId)
{
    public GetPetQuery ToQuery() => new GetPetQuery(PetId);
}