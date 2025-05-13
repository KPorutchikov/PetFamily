namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public record UpdateRequisiteDetailsCommand(Guid VolunteerId, IEnumerable<UpdateRequisiteDetailsCommandDto> Dto);

public record UpdateRequisiteDetailsCommandDto(string Name, string Description);