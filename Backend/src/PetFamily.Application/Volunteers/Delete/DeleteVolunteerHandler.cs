using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteVolunteerRequest request,
        CancellationToken ct = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;


        var result = request.DeletionOptions switch
        {
            DeletionOptions.Hard => await _volunteerRepository.HardDelete(volunteerResult.Value, ct),
            DeletionOptions.Soft => await _volunteerRepository.SoftDelete(volunteerResult.Value, ct),
            _ => throw new NotImplementedException("Invalid deletion option")

        };

        _logger.LogInformation("Volunteer was deleted with id: {Id}.", request.VolunteerId);

        return result;
    }
}

public enum DeletionOptions
{
    Soft,
    Hard
}