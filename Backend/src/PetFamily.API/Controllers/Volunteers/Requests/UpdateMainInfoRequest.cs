using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(
    string FullName,
    string Description,
    string Email,
    string Phone,
    string ExperienceInYears)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new UpdateMainInfoCommand(volunteerId, FullName, Description, Email, Phone, ExperienceInYears);
}