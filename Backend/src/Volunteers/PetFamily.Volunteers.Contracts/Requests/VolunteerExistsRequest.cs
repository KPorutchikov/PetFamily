namespace PetFamily.Volunteers.Contracts.Requests;

public record VolunteerExistsRequest(Guid? volunteerId, string? FullName, string? Email);