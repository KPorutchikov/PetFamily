using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Volunteers.Application.Volunteers.Delete;

public class DeleteVolunteerSoftHandler : ICommandHandler<Guid,DeleteVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerSoftHandler> _logger;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVolunteerSoftHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerSoftHandler> logger, 
        IValidator<DeleteVolunteerCommand> validator,
        [FromKeyedServices(Modules.Volunteers)]IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
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

        var result = _volunteerRepository.SoftDelete(volunteerResult.Value, ct);

        await _unitOfWork.SaveChanges(ct);
        
        _logger.LogInformation("Volunteer was deleted (soft) with id: {Id}.", command.VolunteerId);

        return result;
    }
}