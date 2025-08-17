using CSharpFunctionalExtensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Shared.Core.Shared;

public class Requisites : ComparableValueObject
{
    public string Name { get; }   
    public string Description { get; }

    private Requisites(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<Requisites, Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("name");
        
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("description");
        
        return new Requisites(name, description);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}