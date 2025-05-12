namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand(
    string FullName,
    string Email,
    string Description,
    string Phone,
    string ExperienceInYears);