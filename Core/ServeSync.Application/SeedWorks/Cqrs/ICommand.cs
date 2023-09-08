using MediatR;

namespace ServeSync.Application.SeedWorks.Cqrs;

public interface ICommand : IRequest
{
    
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}