using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Volunteers.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;