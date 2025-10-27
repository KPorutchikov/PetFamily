using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts.Responses;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Login;

public class LoginHandler : ICommandHandler<LoginResponse, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(UserManager<User> userManager, ITokenProvider tokenProvider, ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }
        
    public async Task<Result<LoginResponse, ErrorList>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Errors.General.NotFound().ToErrorList();
        }
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
        {
            return Errors.General.ValueIsInvalid().ToErrorList();
        }

        var accessToken = await _tokenProvider.GenerateAccessToken(user, cancellationToken);
        if (accessToken.IsFailure)
        {
            return accessToken.Error;
        }

        var refreshToken = _tokenProvider.GenerateRefreshToken(user, accessToken.Value.Jti);
        if (refreshToken.IsFailure)
        {
            return refreshToken.Error;
        }
        var loginResponce = new LoginResponse(accessToken.Value.AccessToken, refreshToken.Value);

        return loginResponce;
    }
}