using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<CreateVolunteerRequest> _validator;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
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
            return Errors.Volunteer.AlreadyExist();

        var volunteerResult = Volunteer.Create(
            VolunteerId.NewId(),
            fullName,
            email,
            description,
            phone,
            experienceInYears);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        await _volunteerRepository.Add(volunteerResult.Value, ct);

        return (Guid)volunteerResult.Value.Id;
    }
}