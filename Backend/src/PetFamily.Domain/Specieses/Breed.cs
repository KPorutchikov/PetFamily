using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Specieses;

public class Breed : Entity<BreedId>
{
    public string Name { get; private set; }
    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }
    public static Result<Breed, Error> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("name");
        
        return new Breed(id, name);
    }
}