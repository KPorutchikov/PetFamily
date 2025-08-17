using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Species.Domain.Models;

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