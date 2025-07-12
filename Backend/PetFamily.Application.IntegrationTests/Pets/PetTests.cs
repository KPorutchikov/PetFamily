using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species.ValueObjects;
using PetFamily.Domain.Specieses;
using PetFamily.Infrastructure;
using Xunit;

namespace PetFamily.Application.IntegrationTests.Pets;

public class PetTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    private readonly AddPetHandler _sut;
    private readonly AddPetPhotosHandler _sutPetPhotos;
    private readonly CreateVolunteerHandler _volunteerHandler;
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceScope _scope;
    private readonly Fixture _fixture;

    public PetTests(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<AddPetHandler>();
        _sutPetPhotos = _scope.ServiceProvider.GetRequiredService<AddPetPhotosHandler>();
        _volunteerHandler = _scope.ServiceProvider.GetRequiredService<CreateVolunteerHandler>();
        _fixture = new Fixture();
    }
    
    [Fact]
    public async Task Add_pet_to_database()
    {
        // Arrange
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        
        var speciesId = CreateSpecies();
        var breedId = CreateBreed(speciesId);
        var petCommand = _fixture.CreatePet(volunteerId, speciesId, breedId);
        
        // Act
        var result = await _sut.Handle(petCommand, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var volunteer = _dbContext.Volunteers.Include(p => p.Pets).FirstOrDefault();
        volunteer!.Pets.FirstOrDefault(p => p.Id.Value == result.Value).Should().NotBeNull();
    }

    [Fact]
    public async Task Add_pet_photo_to_minio_and_database()
    {
        // Arrange
        var filePath = FilePath.Create(Guid.NewGuid(), ".jpg").Value;
        _factory.SetupSuccessFileProviderMock(filePath);
        var volunteerId = await _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None);
        var pet = CreatePet(volunteerId.Value).Result;
        var command = _fixture.CreatePetPhotosCommand(pet, filePath);

        // Act
        var result = await _sutPetPhotos.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    private async Task<Guid> CreatePet(Guid volunteerId)
    {
        var speciesId = CreateSpecies();
        var breedId = CreateBreed(speciesId);
        var petCommand = _fixture.CreatePet(volunteerId, speciesId, breedId);

        var pet = await _sut.Handle(petCommand, CancellationToken.None);
        if (!pet.IsSuccess)
            return Guid.Empty;

        return pet.Value;
    }

    private Guid CreateSpecies()
    {
        var command = _fixture.CreateSpecies();
        var species = Domain.Specieses.Species.Create(SpeciesId.NewId(), command.Name, command.Title).Value;
        
        _dbContext.Species.Add(species);

        _dbContext.SaveChangesAsync();
        
        return species.Id;
    }

    private Guid CreateBreed(Guid speciesId)
    {
        var command = _fixture.CreateBreed(speciesId);
        var breed = Breed.Create(BreedId.NewId(), command.Name!).Value;

        var species = _dbContext.Species.FirstOrDefault(s => s.Id == command.SpeciesId);
        if (species == null)
            return Guid.Empty;

        species.AddBreed(breed);
        
        _dbContext.SaveChanges();

        return breed.Id;
    }

    
    public Task InitializeAsync() => Task.CompletedTask;
    
    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}