using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record SerialNumber
{
    public int Value { get; }

    public SerialNumber(int value)
    {
        Value = value;
    }
    
    public static Result<SerialNumber, Error> Create(int number)
    {
        if (number <= 0)
            return Errors.General.ValueIsInvalid("Serial number");
        
        return new SerialNumber(number);
    }
}