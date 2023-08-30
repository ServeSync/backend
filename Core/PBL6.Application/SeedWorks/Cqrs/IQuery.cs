using MediatR;

namespace PBL6.Application.SeedWorks.Cqrs;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    
}