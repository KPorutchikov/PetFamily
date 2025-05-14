using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public class UpdateRequisiteDetailsHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRequisiteDetailsHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger, 
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateRequisiteDetailsCommand command,
        CancellationToken ct = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var requisiteDetails = command.Dto.Select(s => Requisites.Create(s.Name, s.Description).Value).ToList();

        volunteerResult.Value.AddRequisiteDetails(new RequisiteDetails() { RequisitesList = requisiteDetails });

        await _unitOfWork.SaveChanges(ct);

        _logger.LogInformation("Requisite details were successfully updated.");

        return command.VolunteerId;
    }
}