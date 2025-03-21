using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository, ILogger<CreateVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(CreateVolunteerRequest request, CancellationToken ct = default)
    {
        var fullName = FullName.Create(request.FullName).Value;
        var email = Email.Create(request.Email).Value;
        var description = Description.Create(request.Description).Value;
        var phone = Phone.Create(request.Phone).Value;
        var experienceInYears = ExperienceInYears.Create(request.ExperienceInYears).Value;

        var volunteer = await _volunteerRepository.GetByFullName(request.FullName, ct);

        if (volunteer.IsSuccess)
        {
            _logger.LogError("Failed to create. Volunteer is exists: {fullName}", fullName.Value);
            return Errors.Volunteer.AlreadyExist();
        }

        var volunteerId = VolunteerId.NewId();
        
        var volunteerResult = Volunteer.Create(
            volunteerId,
            fullName,
            email,
            description,
            phone,
            experienceInYears);

        if (volunteerResult.IsFailure)
        {
            _logger.LogError("Failed to create volunteer: {@error}", volunteerResult.Error);
            return volunteerResult.Error;
        }

        await _volunteerRepository.Add(volunteerResult.Value, ct);
        
        _logger.LogInformation("Created volunteer [{fullName}] with [{@volunteer}]", fullName.Value, volunteerResult.Value);

        return (Guid)volunteerResult.Value.Id;
    }
}