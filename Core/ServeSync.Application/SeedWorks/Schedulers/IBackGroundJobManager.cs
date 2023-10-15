using System.Linq.Expressions;

namespace ServeSync.Application.SeedWorks.Schedulers;

public interface IBackGroundJobManager
{
    string Fire(Expression<Action> factory);
    
    string Fire<T>(Expression<Action<T>> factory);
    
    string Fire(IBackGroundJob job);

    string FireAfter<T>(Expression<Action> factory, TimeSpan timeSpan);
    
    string FireAfter<T>(Expression<Action<T>> factory, TimeSpan timeSpan);
    
    string FireAfter(IBackGroundJob job, TimeSpan timeSpan);
    
    string FireAt<T>(Expression<Action> factory, DateTime dateTime);
    
    string FireAt<T>(Expression<Action<T>> factory, DateTime dateTime);
    
    string FireAt(IBackGroundJob job, DateTime dateTime);
    
    bool Remove(string jobId);
}