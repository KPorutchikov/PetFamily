namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoRequest(Guid VolunteerId, UpdateMainInfoDto Dto);

public record UpdateMainInfoDto(string FullName, string Description, string Email, string Phone, string ExperienceInYears);
