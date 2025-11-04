using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Roles;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Register;

public class RegisterParticipantHandler : ICommandHandler<RegisterParticipantCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParticipantAccountManager _participantAccountManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RegisterParticipantHandler> _logger;

    public RegisterParticipantHandler(
        [FromKeyedServices(Modules.Accounts)] IUnitOfWork unitOfWork,
        IParticipantAccountManager participantAccountManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<RegisterParticipantHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _participantAccountManager = participantAccountManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        RegisterParticipantCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var participantRole = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);
        if (participantRole is null)
            return Errors.General.NotFound(null).ToErrorList();

        var participantUser = User.CreateParticipant(command.UserName, command.Email, participantRole);
        
        var participantExists = await _participantAccountManager.GetParticipantIfExists(participantUser);
        if (participantExists != null)
            return Errors.General.AlreadyExist().ToErrorList();

        var resultUser = await _userManager.CreateAsync(participantUser, command.Password);
        if (!resultUser.Succeeded)
            return new ErrorList(resultUser.Errors.Select(e => Error.Failure(e.Code, e.Description)));

        await _participantAccountManager.CreateParticipantAccount(new ParticipantAccount(command.UserName, participantUser));
        
        await _unitOfWork.SaveChanges(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        
        _logger.LogInformation("User created: {userName} a new participant account with password.", command.UserName);
        
        return Result.Success<ErrorList>();
    }
}