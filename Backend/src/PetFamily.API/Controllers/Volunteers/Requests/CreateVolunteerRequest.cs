using PetFamily.Application.Volunteers.Create;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    string FullName,
    string Email,
    string Description,
    string Phone,
    string ExperienceInYears)
{
    public CreateVolunteerCommand ToCommand() =>
        new CreateVolunteerCommand(FullName, Email, Description, Phone, ExperienceInYears);
}