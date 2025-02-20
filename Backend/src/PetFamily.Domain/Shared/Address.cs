using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public class Address : ComparableValueObject
{
    public string City { get; } = default!;
    public string Street { get; } = default!;
    public string? HouseNumber { get; }
    public string? ApartmentNumber { get; }

    private Address(string city, string street, string houseNumber, string apartmentNumber)
    {
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        ApartmentNumber = apartmentNumber;
    }

    public static Result<Address> Create(string city, string street, string houseNumber, string apartmentNumber)
    {
        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>($"{nameof(city)} is not be empty");
        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>($"{nameof(street)} is not be empty");
        if (string.IsNullOrWhiteSpace(houseNumber))
            return Result.Failure<Address>($"{nameof(houseNumber)} is not be empty");
        if (string.IsNullOrWhiteSpace(apartmentNumber))
            return Result.Failure<Address>($"{nameof(apartmentNumber)} is not be empty");

        return Result.Success(new Address(city, street, houseNumber, apartmentNumber));
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return HouseNumber == null ? "" : HouseNumber;
        yield return ApartmentNumber == null ? "" : ApartmentNumber;
    }
}