namespace PetFamily.Application.Dtos;

public class VolunteerDto
{
    public Guid Id { get; init; }
    
    public string FullName { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public int ExperienceInYears { get; init; }
    
    public string Email { get; init; } = string.Empty;
    
    public string Phone { get; init; } = string.Empty;
}