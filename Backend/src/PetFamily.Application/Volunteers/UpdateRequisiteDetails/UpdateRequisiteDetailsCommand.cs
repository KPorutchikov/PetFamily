using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public record UpdateRequisiteDetailsCommand(Guid VolunteerId, IEnumerable<UpdateRequisiteDetailsCommandDto> Dto) : ICommand;

public record UpdateRequisiteDetailsCommandDto(string Name, string Description);