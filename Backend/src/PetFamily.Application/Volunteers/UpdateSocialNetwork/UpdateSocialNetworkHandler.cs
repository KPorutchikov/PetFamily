using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetwork;

public class UpdateSocialNetworkHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateSocialNetworkCommand> _validator;

    public UpdateSocialNetworkHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger, IUnitOfWork unitOfWork, 
        IValidator<UpdateSocialNetworkCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworkCommand command,
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

        var socialNetworks = command.Dto.Select(s => SocialNetwork.Create(s.Link, s.Title).Value).ToList();

        volunteerResult.Value.AddSocialNetworkDetails(new SocialNetworkDetails() { SocialNetworks = socialNetworks });

        await _unitOfWork.SaveChanges(ct);

        _logger.LogInformation("SocialNetwork was successfully updated.");

        return command.VolunteerId;
    }
}