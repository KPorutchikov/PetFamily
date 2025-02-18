using CSharpFunctionalExtensions;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Species;

public class Species: Entity<SpeciesId>
{
    private List<Breed> _breeds = [];
    
    public IReadOnlyList<Breed> Breeds => _breeds;
    public string Name { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    
    private Species(SpeciesId id) : base(id) { }
    private Species(SpeciesId id, string name, string title, List<Breed> breeds) : base(id)
    {
        Name = name;
        Title = title;
        _breeds = breeds;
    }

    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }

    public static Result<Species> Create(SpeciesId id, string name, string title, List<Breed> breeds)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Species>($"{nameof(name)} is not be empty");

        return Result.Success(new Species(id, name, title, breeds));
    }
}