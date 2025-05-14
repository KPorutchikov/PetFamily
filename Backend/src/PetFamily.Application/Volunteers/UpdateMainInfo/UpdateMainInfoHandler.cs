using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<UpdateMainInfoCommand> _validator;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMainInfoHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<UpdateMainInfoCommand> validator,
        ILogger<UpdateMainInfoHandler> logger, 
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
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

        volunteerResult.Value.UpdateMainInfo(
            FullName.Create(command.FullName).Value,
            Description.Create(command.Description).Value,
            Email.Create(command.Email).Value,
            Phone.Create(command.Phone).Value,
            ExperienceInYears.Create(command.ExperienceInYears).Value);

        await _unitOfWork.SaveChanges(ct);

        _logger.LogInformation("Volunteer was successfully updated.");
        return command.VolunteerId;
    }
}