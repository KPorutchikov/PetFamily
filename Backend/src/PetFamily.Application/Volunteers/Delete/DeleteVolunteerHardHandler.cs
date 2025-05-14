using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class DeleteVolunteerHardHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerHardHandler> _logger;
    private readonly IValidator<DeleteVolunteerCommand> _validator;

    public DeleteVolunteerHardHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHardHandler> logger, 
        IValidator<DeleteVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken ct = default)
    {
        var validationResult= await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
 
        var result = _volunteerRepository.HardDelete(volunteerResult.Value, ct);

        _logger.LogInformation("Volunteer was deleted (hard) with id: {Id}.", command.VolunteerId);

        return result;
    }
}