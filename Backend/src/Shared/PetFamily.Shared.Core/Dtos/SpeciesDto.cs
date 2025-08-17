namespace PetFamily.Shared.Core.Dtos;

public class SpeciesDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string Title { get; init; } = string.Empty;
}