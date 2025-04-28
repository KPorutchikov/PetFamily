using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken ct = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        volunteerResult.Value.UpdateMainInfo(
            FullName.Create(request.Dto.FullName).Value,
            Description.Create(request.Dto.Description).Value,
            Email.Create(request.Dto.Email).Value,
            Phone.Create(request.Dto.Phone).Value,
            ExperienceInYears.Create(request.Dto.ExperienceInYears).Value);

        var result = _volunteerRepository.Save(volunteerResult.Value, ct);

        _logger.LogInformation("Volunteer was successfully updated.");
        return result;
    }
}