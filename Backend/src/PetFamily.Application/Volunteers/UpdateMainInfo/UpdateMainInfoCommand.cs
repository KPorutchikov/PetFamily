namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid VolunteerId, string FullName, string Description, 
                                    string Email, string Phone, string ExperienceInYears);