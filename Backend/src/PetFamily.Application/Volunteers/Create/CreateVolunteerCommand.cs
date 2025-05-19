using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerCommand(
    string FullName,
    string Email,
    string Description,
    string Phone,
    string ExperienceInYears) : ICommand;