using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.Application.Volunteers.Delete;

public record DeleteVolunteerRequest(Guid VolunteerId, DeletionOptions DeletionOptions);