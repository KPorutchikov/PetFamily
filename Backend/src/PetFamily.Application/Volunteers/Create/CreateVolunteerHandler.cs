using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(
        IVolunteerRepository volunteerRepository, 
        ILogger<CreateVolunteerHandler> logger, 
        IUnitOfWork unitOfWork, 
        IValidator<CreateVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(CreateVolunteerCommand command, CancellationToken ct = default)
    {
        var validationResult= await _validator.ValidateAsync(command, ct);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var fullName = FullName.Create(command.FullName).Value;
        var email = Email.Create(command.Email).Value;
        var description = Description.Create(command.Description).Value;
        var phone = Phone.Create(command.Phone).Value;
        var experienceInYears = ExperienceInYears.Create(command.ExperienceInYears).Value;

        var volunteer = await _volunteerRepository.GetByFullName(command.FullName, ct);

        if (volunteer.IsSuccess)
        {
            _logger.LogError("Failed to create. Volunteer is exists: {fullName}", fullName.Value);
            return Errors.Volunteer.AlreadyExist().ToErrorList();
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
            return volunteerResult.Error.ToErrorList();
        }

        _volunteerRepository.Add(volunteerResult.Value, ct);
        
        await _unitOfWork.SaveChanges(ct);
        
        _logger.LogInformation("Created volunteer [{fullName}] with [{@volunteer}]", fullName.Value, volunteerResult.Value);

        return (Guid)volunteerResult.Value.Id;
    }
}