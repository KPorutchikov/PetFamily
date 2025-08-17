using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetPet;

public record GetPetQuery(Guid PetId) : IQuery;