using MediatR;

namespace ServeSync.Application.SeedWorks.Cqrs;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    
}