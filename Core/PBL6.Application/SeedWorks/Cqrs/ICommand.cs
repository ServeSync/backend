using MediatR;

namespace PBL6.Application.SeedWorks.Cqrs;

public interface ICommand : IRequest
{
    
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}