using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerSoftHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerSoftHandler> _logger;

    public DeleteVolunteerSoftHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerSoftHandler> logger)
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

        var result = await _volunteerRepository.SoftDelete(volunteerResult.Value, ct);

        _logger.LogInformation("Volunteer was deleted (soft) with id: {Id}.", command.VolunteerId);

        return result;
    }
}