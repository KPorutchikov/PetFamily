using CSharpFunctionalExtensions;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Domain.Species;

public class Breed : Entity<BreedId>
{
    public string Name { get; private set; }
    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }

    public static Result<Breed> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>("Name is not be empty");
        
        return Result.Success(new Breed(id, name));
    }
}