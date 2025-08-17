using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.IntegrationTests.Extensions;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;
using PetFamily.Species.Infrastructure.Database;
using PetFamily.Volunteers.Application.Volunteers.AddPet;
using PetFamily.Volunteers.Application.Volunteers.AddPetPhotos;
using PetFamily.Volunteers.Application.Volunteers.Create;
using PetFamily.Volunteers.Application.Volunteers.EditPet;
using PetFamily.Volunteers.Application.Volunteers.EditPet.DeletePet;
using PetFamily.Volunteers.Application.Volunteers.EditPet.UpdatePet;
using PetFamily.Volunteers.Infrastructure.Database;
using Xunit;

namespace PetFamily.Application.IntegrationTests.Volunteers;

public class PetTests : IClassFixture<VolunteerIntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly VolunteerIntegrationTestsWebFactory _factory;
    private readonly AddPetHandler _sutAddPet;
    private readonly CreateVolunteerHandler _volunteerHandler;
    private readonly VolunteerDbContext _dbContextVolunteer;
    private readonly SpeciesDbContext _dbContextSpecies;
    private readonly IServiceScope _scope;
    private readonly Fixture _fixture;

    public PetTests(VolunteerIntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _dbContextSpecies = _scope.ServiceProvider.GetRequiredService<SpeciesDbContext>();
        _dbContextVolunteer = _scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
        _volunteerHandler = _scope.ServiceProvider.GetRequiredService<CreateVolunteerHandler>();
        _sutAddPet = _scope.ServiceProvider.GetRequiredService<AddPetHandler>();
    }

    [Fact]
    public async Task Add_pet_to_database()
    {
        // Arrange
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;

        var speciesId = await CreateSpecies();
        var breedId = CreateBreed(speciesId);
        var petCommand = _fixture.CreatePet(volunteerId, speciesId, breedId);

        // Act
        var result = await _sutAddPet.Handle(petCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var volunteer = _dbContextVolunteer.Volunteers.Include(p => p.Pets).FirstOrDefault();
        volunteer!.Pets.FirstOrDefault(p => p.Id.Value == result.Value).Should().NotBeNull();
    }

    [Fact]
    public async Task Add_pet_photo_to_minio_and_database()
    {
        // Arrange
        var sut = _scope.ServiceProvider.GetRequiredService<AddPetPhotosHandler>();
        var filePath = FilePath.Create(Guid.NewGuid(), ".jpg").Value;
        _factory.SetupSuccessFileProviderMock(filePath);
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        var pet = CreatePet(volunteerId).Result;
        var command = _fixture.CreatePetPhotosCommand(pet, filePath);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var petResult = await _dbContextVolunteer.Volunteers.FirstOrDefaultAsync();
        petResult!.Pets.FirstOrDefault()!.Files!.Select(p => p.PathToStorage.Path).FirstOrDefault().Should()
            .NotBeNull();
    }

    [Fact]
    public async Task Delete_soft_pet_in_database()
    {
        // Arrange
        var sut = _scope.ServiceProvider.GetRequiredService<DeletePetSoftHandler>();
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        var petId = CreatePet(volunteerId).Result;
        var command = new DeletePetCommand(petId);

        // Act
        var resultDelete = await sut.Handle(command, CancellationToken.None);

        // Assert
        resultDelete.Value.Should().NotBeEmpty();
        _dbContextVolunteer.Volunteers.FirstOrDefault()!.Pets.FirstOrDefault()!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_hard_pet_in_database()
    {
        // Arrange
        var sut = _scope.ServiceProvider.GetRequiredService<DeletePetHardHandler>();
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        var petId = CreatePet(volunteerId).Result;
        var command = new DeletePetCommand(petId);

        // Act
        var resultDelete = await sut.Handle(command, CancellationToken.None);

        // Assert
        resultDelete.Value.Should().NotBeEmpty();
        resultDelete.Value.Should().Be(petId);
        _dbContextVolunteer.Volunteers.FirstOrDefault()!.Pets.FirstOrDefault(p => p.Id.Value == petId).Should().Be(null);
    }

    [Fact]
    public async Task Update_pet_in_database()
    {
        // Arrange
        var sut = _scope.ServiceProvider.GetRequiredService<UpdatePetHandler>();
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        var speciesId = await CreateSpecies();
        var breedId = CreateBreed(speciesId);
        var pet = await _sutAddPet.Handle(_fixture.CreatePet(volunteerId, speciesId, breedId), CancellationToken.None);
        var commandUpdatePet = _fixture.UpdatePetCommand(pet.Value, speciesId, breedId);

        // Act
        var result = await sut.Handle(commandUpdatePet, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeEmpty();
        _dbContextVolunteer.Volunteers.FirstOrDefault()!.Pets.FirstOrDefault()!.Name.Should().Be(commandUpdatePet.Name);
    }

    [Fact]
    public async Task Move_pet_position_in_database()
    {
        // Arrange
        List<Guid> petsId = [];
        var sut = _scope.ServiceProvider.GetRequiredService<MovePetHandler>();
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        var speciesId = await CreateSpecies();
        var breedId = CreateBreed(speciesId);

        foreach (var num in Enumerable.Range(0, 10))
            petsId.Add(_sutAddPet.Handle(_fixture.CreatePet(volunteerId, speciesId, breedId), CancellationToken.None)
                .Result.Value);

        var command = new MovePetCommand(petsId[1], 5);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeEmpty();

        var resultAfterMove = _dbContextVolunteer.Volunteers.FirstOrDefault();
        resultAfterMove.Pets[0].SerialNumber.Value.Should().Be(1);
        resultAfterMove.Pets[1].SerialNumber.Value.Should().Be(5);
        resultAfterMove.Pets[4].SerialNumber.Value.Should().Be(4);
        resultAfterMove.Pets[5].SerialNumber.Value.Should().Be(6);
        resultAfterMove.Pets[9].SerialNumber.Value.Should().Be(10);
    }

    private async Task<Guid> CreatePet(Guid volunteerId)
    {
        var speciesId = await CreateSpecies();
        var breedId = CreateBreed(speciesId);
        var petCommand = _fixture.CreatePet(volunteerId, speciesId, breedId);

        var pet = await _sutAddPet.Handle(petCommand, CancellationToken.None);
        if (!pet.IsSuccess)
            return Guid.Empty;

        return pet.Value;
    }

    private async Task<Guid> CreateSpecies()
    {
        var command = _fixture.CreateSpecies();
        var species = Species.Domain.Models.Species.Create(SpeciesId.NewId(), command.Name, command.Title).Value;

        _dbContextSpecies.Add(species);

        await _dbContextVolunteer.SaveChangesAsync();

        return species.Id;
    }

    private Guid CreateBreed(Guid speciesId)
    {
        var command = _fixture.CreateBreed(speciesId);
        var breed = Species.Domain.Models.Breed.Create(BreedId.NewId(), command.Name!).Value;

        var species = _dbContextSpecies.Species.FirstOrDefault(s => s.Id == command.SpeciesId);
        if (species == null)
            return Guid.Empty;

        species.AddBreed(breed);

        _dbContextVolunteer.SaveChanges();

        return breed.Id;
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}