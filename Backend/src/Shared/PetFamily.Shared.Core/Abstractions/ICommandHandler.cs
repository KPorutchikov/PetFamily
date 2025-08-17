using CSharpFunctionalExtensions;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Shared.Core.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public  Task<Result<TResponse, ErrorList>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public  Task<UnitResult<ErrorList>> Handle(TCommand command, CancellationToken cancellationToken);
}