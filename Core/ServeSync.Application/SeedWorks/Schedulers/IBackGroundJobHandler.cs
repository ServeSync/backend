using MediatR;

namespace ServeSync.Application.SeedWorks.Schedulers;

public interface IBackGroundJobHandler<in T> : INotificationHandler<T> where T : IBackGroundJob
{
    
}