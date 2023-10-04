using MediatR;

namespace ServeSync.Application.SeedWorks.Cqrs;

public interface ICommandBase
{
    
}

public interface ICommand : ICommandBase, IRequest
{
    
}

public interface ICommand<out TResponse> : ICommandBase, IRequest<TResponse>
{
    
}