using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Create;

public record CreateVolunteerCommand(
    string FullName,
    string Email,
    string Description,
    string Phone,
    string ExperienceInYears) : ICommand;