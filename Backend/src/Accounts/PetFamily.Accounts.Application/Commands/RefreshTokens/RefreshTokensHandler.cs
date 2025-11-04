using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Models;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.RefreshTokens;

public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensHandler(
        IRefreshSessionManager refreshSessionManager, 
        ITokenProvider tokenProvider, 
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<LoginResponse, ErrorList>> Handle(RefreshTokensCommand command, CancellationToken cancellationToken)
    {
        var oldRefreshSession = await _refreshSessionManager.GetByRefreshToken(command.RefreshToken, cancellationToken);
        if (oldRefreshSession.IsFailure)
            return oldRefreshSession.Error.ToErrorList();

        if (oldRefreshSession.Value.ExpiresIn < DateTime.UtcNow)
            return Errors.Tokens.ExpiredToken().ToErrorList();

        var userClaims = await _tokenProvider.GetUserClaims(command.AccessToken, cancellationToken);
        if (userClaims.IsFailure)
            return Errors.Tokens.InvalidToken().ToErrorList();

        var userIdString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
            return Errors.General.ValueIsInvalid().ToErrorList();

        if (oldRefreshSession.Value.UserId != userId)
            return Errors.Tokens.InvalidToken().ToErrorList();

        var userJtiString = userClaims.Value.FirstOrDefault(c => c.Type == CustomClaims.Jti)?.Value;
        if (!Guid.TryParse(userJtiString, out var userJtiGuid))
            return Errors.General.ValueIsInvalid().ToErrorList();
        
        if (oldRefreshSession.Value.Jti != userJtiGuid)
            return Errors.Tokens.InvalidToken().ToErrorList();

        _refreshSessionManager.Delete(oldRefreshSession.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        var accessToken = _tokenProvider.GenerateAccessToken(oldRefreshSession.Value.User, cancellationToken).Result;
        var refreshToken = _tokenProvider.GenerateRefreshToken(oldRefreshSession.Value.User, accessToken.Value.Jti);

        return new LoginResponse(accessToken.Value.AccessToken, refreshToken.Value);
    }
}