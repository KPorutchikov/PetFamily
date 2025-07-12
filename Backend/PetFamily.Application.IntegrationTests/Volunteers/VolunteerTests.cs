using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Infrastructure;
using Xunit;

namespace PetFamily.Application.IntegrationTests.Volunteers;

public class VolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    private readonly CreateVolunteerHandler _sut;
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceScope _scope;
    private readonly Fixture _fixture;

    public VolunteerTests(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<CreateVolunteerHandler>();
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
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var volunteer = await _dbContext.Volunteers.Where(v => v.FullName.Value == command.FullName).FirstOrDefaultAsync();
        volunteer.Should().NotBeNull();
    }
    
    [Fact]
    public void Test2()
    {
    }

    [Fact]
    public void Test3()
    {
    }

    public Task InitializeAsync() => Task.CompletedTask;
    
    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.ResetDatabaseAsync();
    }
}