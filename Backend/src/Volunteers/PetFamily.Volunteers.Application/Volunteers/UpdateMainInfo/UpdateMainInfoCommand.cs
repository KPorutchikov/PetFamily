using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid VolunteerId, string FullName, string Description, 
        string Email, string Phone, string ExperienceInYears) : ICommand;