using PetFamily.Domain.Shared;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Domain.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Volunteers_Return_Success_Result()
    {
        var fullName = FullName.Create("Иванов Иван").Value;
        var email = Email.Create("ivanov@gmail.com").Value;
        var description = Description.Create("Любит домашних животных").Value;
        var phone = Phone.Create("8-095-123-45-67").Value;
        var experienceInYears = ExperienceInYears.Create("5").Value;
        var volunteer = Volunteer.Create(VolunteerId.NewId(), fullName, email, description, phone, experienceInYears)
            .Value;

        var petBreed = PetBreed.Create(Guid.NewGuid(), Guid.NewGuid()).Value;
        var address = Address.Create("Москва", "пр.Ленинский", "15", "45").Value;
        var petStatus = PetStatus.Create(PetStatus.Status.HomeSeeking).Value;
        var petId = PetId.NewId();
        var pet = Pet.Create(petId, "Ушастик", petBreed, "Чих-пых", "коричневый", 15, 6, "здоровый", address,
            "8-095-412-16-99", true, DateOnly.Parse("2020-06-01"), true, petStatus).Value;

        // arrange
        var result = volunteer.AddPet(pet);

        // assert
        Assert.True(result.IsSuccess);
        Assert.Equal(volunteer.Pets.FirstOrDefault(v => v.Id.Value == petId.Value).Id, petId);
    }

    [Fact]
    public void Move_Volunteers_Pets_Return_Success_Result()
    {
        var fullName = FullName.Create("Иванов Иван").Value;
        var email = Email.Create("ivanov@gmail.com").Value;
        var description = Description.Create("Любит домашних животных").Value;
        var phone = Phone.Create("8-095-123-45-67").Value;
        var experienceInYears = ExperienceInYears.Create("5").Value;
        var volunteer = Volunteer.Create(VolunteerId.NewId(), fullName, email, description, phone, experienceInYears).Value;

        var petBreed = PetBreed.Create(Guid.NewGuid(), Guid.NewGuid()).Value;
        var address = Address.Create("Москва", "пр.Ленинский", "15", "45").Value;
        var petStatus = PetStatus.Create(PetStatus.Status.HomeSeeking).Value;

        var pets = Enumerable.Range(1, 10).Select(n =>
            Pet.Create(PetId.NewId(), "Ушастик - " + n.ToString(), petBreed, "Милый чих-пых", "коричневый", 15, 6,
                "здоровый", address, "8-095-412-16-99", true, DateOnly.Parse("2020-06-01"), true, petStatus).Value);

        foreach (Pet _ in pets)
            volunteer.AddPet(_);

        var pet_move_first = volunteer.Pets.FirstOrDefault(p => p.SerialNumber.Value == 2);
        var pet_move_second = volunteer.Pets.FirstOrDefault(p => p.SerialNumber.Value == 9);

        // arrange
        var resultMoveFirst = volunteer.MovePet(pet_move_first, SerialNumber.Create(5).Value);
        var resultMoveSecond = volunteer.MovePet(pet_move_second, SerialNumber.Create(6).Value);
        var resultMoveThird = volunteer.MovePet(pet_move_second, SerialNumber.Create(100).Value);

        // assert
        Assert.True(resultMoveFirst.IsSuccess);
        Assert.Equal(pet_move_first.SerialNumber.Value, 5);

        Assert.True(resultMoveSecond.IsSuccess);
        Assert.Equal(pet_move_second.SerialNumber.Value, 6);

        Assert.True(resultMoveThird.IsFailure);
    }
}