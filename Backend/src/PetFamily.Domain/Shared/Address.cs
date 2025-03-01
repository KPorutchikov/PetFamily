using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public class Address : ComparableValueObject
{
    public string City { get; }
    public string Street { get; }
    public string? HouseNumber { get; }
    public string? ApartmentNumber { get; }

    private Address(string city, string street, string houseNumber, string apartmentNumber)
    {
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        ApartmentNumber = apartmentNumber;
    }

    public static Result<Address, Error> Create(string city, string street, string houseNumber, string apartmentNumber)
    {
        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsInvalid("city");
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsInvalid("street");
        if (string.IsNullOrWhiteSpace(houseNumber))
            return Errors.General.ValueIsInvalid("house number");
        if (string.IsNullOrWhiteSpace(apartmentNumber))
            return Errors.General.ValueIsInvalid("apartment number");

        return new Address(city, street, houseNumber, apartmentNumber);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return HouseNumber ?? "";
        yield return ApartmentNumber ?? "";
    }
}