using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Specieses;

public class Species: Entity<SpeciesId>
{
    private List<Breed> _breeds = [];
    
    public IReadOnlyList<Breed> Breeds => _breeds;
    public string Name { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    
    private Species(SpeciesId id) : base(id) { }
    private Species(SpeciesId id, string name, string title) : base(id)
    {
        Name = name;
        Title = title;
    }

    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }

    public void UpdateBreed(List<Breed> breeds)
    {
        _breeds = breeds;
    }

    public static Result<Species, Error> Create(SpeciesId id, string name, string title)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("name");
        
        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsInvalid("title");

        return new Species(id, name, title);
    }
}