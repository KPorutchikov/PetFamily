namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(string FullName, string Description, 
    string Email, string Phone, string ExperienceInYears);