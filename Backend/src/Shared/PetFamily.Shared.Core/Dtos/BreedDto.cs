namespace PetFamily.Shared.Core.Dtos;

public class BreedDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public Guid SpeciesId { get; init; }
}