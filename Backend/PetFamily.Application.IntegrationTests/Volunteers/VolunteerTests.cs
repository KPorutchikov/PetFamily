using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisiteDetails;
using PetFamily.Application.Volunteers.UpdateSocialNetwork;
using PetFamily.Infrastructure;
using Xunit;

namespace PetFamily.Application.IntegrationTests.Volunteers;

public class VolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    private readonly CreateVolunteerHandler _volunteerHandler;
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceScope _scope;
    private readonly Fixture _fixture;

    public VolunteerTests(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _volunteerHandler = _scope.ServiceProvider.GetRequiredService<CreateVolunteerHandler>();
        _fixture = new Fixture();
    }
    
    [Fact]
    public async Task Add_volunteer_to_database()
    {
        // Arrange
        var command = _fixture.Build<CreateVolunteerCommand>()
            .With(x => x.FullName, "TestVolunteer")
            .With(x => x.Email, "test@test.com")
            .With(x => x.Phone, "123456789")
            .With(x => x.ExperienceInYears, "5")
            .Create();
        
        // Act
        var result = await _volunteerHandler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var volunteer = await _dbContext.Volunteers.Where(v => v.FullName.Value == command.FullName).FirstOrDefaultAsync();
        volunteer.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Update_volunteer_to_database()
    {
        // Arrange
        var sut = _scope.ServiceProvider.GetRequiredService<UpdateMainInfoHandler>();
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        var command = _fixture.Build<UpdateMainInfoCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(x => x.ExperienceInYears, "3")
            .Create();

        // Act
        var result = await sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();

        var volunteer = await _dbContext.Volunteers.Where(v => v.Id == volunteerId).FirstOrDefaultAsync();
        volunteer!.FullName.Value.Should().Be(command.FullName);
        volunteer.Description.Value.Should().Be(command.Description);
        volunteer.Email.Value.Should().Be(command.Email);
        volunteer.Phone.Value.Should().Be(command.Phone);
        volunteer.ExperienceInYears.Value.Should().Be(command.ExperienceInYears);
    }
    
    [Fact]
    public async Task Update_requisite_details_volunteer_to_database()
    {
        // Arrange
        var sut = _scope.ServiceProvider.GetRequiredService<UpdateRequisiteDetailsHandler>();
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        
        IEnumerable<UpdateRequisiteDetailsCommandDto> requisites = new[]
        {
            new UpdateRequisiteDetailsCommandDto("реквизит - 1", "описание реквизита - 1"),
            new UpdateRequisiteDetailsCommandDto("реквизит - 2", "описание реквизита - 2"),
            new UpdateRequisiteDetailsCommandDto("реквизит - 3", "описание реквизита - 3")
        };

        var command = new UpdateRequisiteDetailsCommand(volunteerId, requisites);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();

        var volunteer = await _dbContext.Volunteers.Where(v => v.Id == volunteerId).FirstOrDefaultAsync();
        volunteer!.RequisitesDetails!.RequisitesList.Count.Should().Be(3);
    }
    
    
    [Fact]
    public async Task Update_social_network_volunteer_to_database()
    {
        // Arrange
        var sut = _scope.ServiceProvider.GetRequiredService<UpdateSocialNetworkHandler>();
        var volunteerId = _volunteerHandler.Handle(_fixture.CreateVolunteer(), CancellationToken.None).Result.Value;
        
        IEnumerable<UpdateSocialNetworkCommandDto> networks = new[]
        {
            new UpdateSocialNetworkCommandDto("www.vk.ru", "соц.сеть в контакте"),
            new UpdateSocialNetworkCommandDto("www.facebook.com", "соц.сеть в мордакнига")
        };

        var command = new UpdateSocialNetworkCommand(volunteerId, networks);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();

        var volunteer = await _dbContext.Volunteers.Where(v => v.Id == volunteerId).FirstOrDefaultAsync();
        volunteer!.SocialNetworkDetails.SocialNetworks.Count.Should().Be(2);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    
    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}