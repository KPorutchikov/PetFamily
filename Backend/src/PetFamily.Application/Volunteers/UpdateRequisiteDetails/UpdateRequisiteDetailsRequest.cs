namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public record UpdateRequisiteDetailsRequest(Guid VolunteerId, IEnumerable<UpdateRequisiteDetailsDto> Dto);

public record UpdateRequisiteDetailsDto(string Name, string Description);