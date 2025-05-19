using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateRequisiteDetails;

public class UpdateRequisiteDetailsHandler : ICommandHandler<Guid,UpdateRequisiteDetailsCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateRequisiteDetailsHandler> _logger;
    private readonly IValidator<UpdateRequisiteDetailsCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRequisiteDetailsHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateRequisiteDetailsHandler> logger, 
        IUnitOfWork unitOfWork, 
        IValidator<UpdateRequisiteDetailsCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRequisiteDetailsCommand command,
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

        var requisiteDetails = command.Dto.Select(s => Requisites.Create(s.Name, s.Description).Value).ToList();

        volunteerResult.Value.AddRequisiteDetails(new RequisiteDetails() { RequisitesList = requisiteDetails });

        await _unitOfWork.SaveChanges(ct);

        _logger.LogInformation("Requisite details were successfully updated.");

        return command.VolunteerId;
    }
}