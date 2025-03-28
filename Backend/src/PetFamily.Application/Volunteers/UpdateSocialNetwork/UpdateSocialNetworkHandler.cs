using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetwork;

public class UpdateSocialNetworkHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateSocialNetworkHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateSocialNetworkRequest request,
        CancellationToken ct = default)
    {
        var volunteerResult = await _volunteerRepository.GetById(request.VolunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var socialNetworks = request.Dto.Select(s => SocialNetwork.Create(s.Link, s.Title).Value).ToList();

        volunteerResult.Value.AddSocialNetworkDetails(new SocialNetworkDetails() { SocialNetworks = socialNetworks });

        var result = await _volunteerRepository.Save(volunteerResult.Value, ct);

        _logger.LogInformation("SocialNetwork was successfully updated.");

        return result;
    }
}