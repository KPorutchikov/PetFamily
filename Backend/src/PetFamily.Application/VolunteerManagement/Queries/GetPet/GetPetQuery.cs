using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteerManagement.Queries.GetPet;

public record GetPetQuery(Guid PetId) : IQuery;