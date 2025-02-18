using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record Address
{
    public string City { get; } = default!;
    public string Street { get; } = default!;
    public int? HouseNumber { get; }
    public int? ApartmentNumber { get; }

    private Address(string city, string street, int houseNumber, int apartmentNumber)
    {
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        ApartmentNumber = apartmentNumber;
    }

    public static Result<Address> Create(string city, string street, int houseNumber, int apartmentNumber)
    {
        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>($"{nameof(city)} is not be empty");
        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address>($"{nameof(street)} is not be empty");
        if (houseNumber <= 0)
            return Result.Failure<Address>($"{nameof(houseNumber)} should be more than zero");
        if (apartmentNumber <= 0)
            return Result.Failure<Address>($"{nameof(apartmentNumber)} should be more than zero");

        return Result.Success(new Address(city, street, houseNumber, apartmentNumber));
    }
   
}