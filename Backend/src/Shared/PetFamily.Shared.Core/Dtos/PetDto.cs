namespace PetFamily.Shared.Core.Dtos;

public class PetDto
{
    public Guid Id { get; set; }
    
    public string Volunteer { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Species { get; set; } = string.Empty;
    
    public string Breed { get; set; } = string.Empty;
    
    public string Color { get; set; } = string.Empty;
    
    public float Weight { get; set; }
    
    public float Height { get; set; }
    
    public string HealthInformation { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public bool IsVaccinated { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public string AddressCity { get; set; } = string.Empty;

    public string AddressStreet { get; set; } = string.Empty;

    public PetFileDto[] Files { get; set; } = null!;
}

public class PetFileDto
{
    public string PathToStorage { get; set; } = string.Empty;
}

public class RootValueObject
{
    public List<PetFileDto> Values { get; set; }
}