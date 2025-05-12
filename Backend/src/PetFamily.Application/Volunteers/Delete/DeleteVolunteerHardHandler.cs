using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerHardHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerHardHandler> _logger;

    public DeleteVolunteerHardHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHardHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken ct = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
 
        var result = _volunteerRepository.HardDelete(volunteerResult.Value, ct);

        _logger.LogInformation("Volunteer was deleted (hard) with id: {Id}.", command.VolunteerId);

        return result;
    }
}