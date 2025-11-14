using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Users;

public class GetUserHandler: ICommandHandler<User, GetUserCommand>
{
    private readonly ILogger<GetUserHandler> _logger;
    private readonly IAccountsRepository _accountsRepository;
    private readonly IValidator<GetUserCommand> _validator;


    public GetUserHandler(
        ILogger<GetUserHandler> logger,
        IAccountsRepository accountsRepository,
        IValidator<GetUserCommand> validator)
    {
        _logger = logger;
        _accountsRepository = accountsRepository;
        _validator = validator;
    }
    
    public async Task<Result<User, ErrorList>> Handle(GetUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var user = _accountsRepository.GetUserByUserId(command.UserId, cancellationToken).Result.Value;
        if (user == null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        return user;
    }
}