using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RefreshTokens;
using PetFamily.Accounts.Application.Commands.Register;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Shared.Framework;
using PetFamily.Shared.Framework.Authorization;

namespace PetFamily.Accounts.Presentation;

public class AccountsController : ApplicationController
{
 
    [Permission(Permissions.Volunteer.VolunteerCreate)]
    [HttpPost("test")]
    public IActionResult Test()
    {
        return Ok();
    }
    
    [Permission(Permissions.Volunteer.VolunteerCreate)]
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterParticipantHandler handel,
        CancellationToken cancellationToken)
    {
        var result = await handel.Handle(new RegisterParticipantCommand(request.Email, request.UserName, request.Password), cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handel,
        CancellationToken cancellationToken)
    {
        var result = await handel.Handle(new LoginCommand(request.Email, request.Password), cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromBody] RefreshTokensRequest request,
        [FromServices] RefreshTokensHandler handel,
        CancellationToken cancellationToken)
    {
        var result = await handel.Handle(new RefreshTokensCommand(request.AccessToken, request.RefreshToken), cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}