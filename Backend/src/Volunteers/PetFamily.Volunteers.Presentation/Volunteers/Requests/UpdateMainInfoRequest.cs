using PetFamily.Volunteers.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.Volunteers.Presentation.Volunteers.Requests;

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