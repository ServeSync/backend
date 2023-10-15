using System.Linq.Expressions;
using Hangfire;
using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Infrastructure.HangFire;

public class HangFireBackGroundJobManager : IBackGroundJobManager
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    
    public HangFireBackGroundJobManager(
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJobManager)
    {
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
    }
    
    public string Fire(Expression<Action> factory)
    {
        return _backgroundJobClient.Enqueue(factory);
    }

    public string Fire<T>(Expression<Action<T>> factory)
    {
        return _backgroundJobClient.Enqueue<T>(factory);
    }

    public string Fire(IBackGroundJob job)
    {
        return _backgroundJobClient.Enqueue<IBackGroundJobPublisher>(x => x.Publish(job));
    }

    public string FireAfter<T>(Expression<Action> factory, TimeSpan timeSpan)
    {
        return _backgroundJobClient.Schedule(factory, timeSpan);
    }

    public string FireAfter<T>(Expression<Action<T>> factory, TimeSpan timeSpan)
    {
        return _backgroundJobClient.Schedule(factory, timeSpan);
    }

    public string FireAfter(IBackGroundJob job, TimeSpan timeSpan)
    {
        return _backgroundJobClient.Schedule<IBackGroundJobPublisher>(x => x.Publish(job), timeSpan);
    }

    public string FireAt<T>(Expression<Action> factory, DateTime dateTime)
    {
        return _backgroundJobClient.Schedule(factory, new DateTimeOffset(dateTime));
    }

    public string FireAt<T>(Expression<Action<T>> factory, DateTime dateTime)
    {
        return _backgroundJobClient.Schedule<T>(factory, new DateTimeOffset(dateTime));
    }
    
    public string FireAt(IBackGroundJob job, DateTime dateTime)
    {
        return _backgroundJobClient.Schedule<IBackGroundJobPublisher>(x => x.Publish(job), new DateTimeOffset(dateTime));
    }

    public bool Remove(string jobId)
    {
        return _backgroundJobClient.Delete(jobId);
    }
}