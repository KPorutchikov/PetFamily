using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public class UpdateRequisiteDetailsHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateRequisiteDetailsHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateRequisiteDetailsRequest request,
        CancellationToken ct = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var requisiteDetails = request.Dto.Select(s => Requisites.Create(s.Name, s.Description).Value).ToList();

        volunteerResult.Value.AddRequisiteDetails(new RequisiteDetails() { RequisitesList = requisiteDetails });

        var result = await _volunteerRepository.Save(volunteerResult.Value, ct);

        _logger.LogInformation("Requisite details were successfully updated.");

        return result;
    }
}