using AutoFixture;
using PetFamily.Application.Species.AddBreed;
using PetFamily.Application.Species.AddSpecies;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.EditPet.UpdatePet;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.IntegrationTests;

public static class FixtureExtensions
{
    public static CreateVolunteerCommand CreateVolunteer(
        this IFixture fixture)
    {
        return fixture.Build<CreateVolunteerCommand>()
            .With(x => x.Email, "test@test.com")
            .With(x => x.Description, "Testing volunteer ...")
            .With(x => x.Phone, "123456789")
            .With(x => x.ExperienceInYears, "5")
            .Create();
    }

    public static AddPetCommand CreatePet(
        this IFixture fixture,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId )
    {
        return fixture.Build<AddPetCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(x => x.SpeciesId, speciesId)
            .With(x => x.BreedId, breedId)
            .With(x => x.Status, 1)
            .Create();
    }

    public static AddPetPhotosCommand CreatePetPhotosCommand(this IFixture fixture, Guid petId, FilePath fileName)
    {
        fixture.Register<Stream>(() => new MemoryStream());
        
        List<CreateFileDto> files = [];

        var stream = fixture.Create<Stream>();
        
        files.Add(new CreateFileDto(stream, fileName.Path));
            
        return fixture.Build<AddPetPhotosCommand>()
            .With(x => x.PetId, petId)
            .With(x => x.Files, files)
            .Create();
    }
    
    public static UpdatePetCommand UpdatePetCommand(this IFixture fixture, Guid petId, Guid speciesId, Guid breedId)
    {
        return fixture.Build<UpdatePetCommand>()
            .With(x => x.PetId, petId)
            .With(x => x.SpeciesId, speciesId)
            .With(x => x.BreedId, breedId)
            .With(x => x.Status, 1)
            .Create();
    }
    
    public static AddSpeciesCommand CreateSpecies(this IFixture fixture)
    {
        return fixture.Build<AddSpeciesCommand>()
            .Create();
    }

    public static AddBreedCommand CreateBreed(this IFixture fixture, Guid speciesId)
    {
        return fixture.Build<AddBreedCommand>()
            .With(x => x.SpeciesId, speciesId)
            .Create();
    }
}