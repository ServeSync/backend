using System.Reflection;

namespace ServeSync.Application.Caching;

public abstract class CachingManager<T> where T : class
{
    protected const int DefaultCacheDurationInDay = 1;
    protected readonly string ResourceName;

    protected CachingManager()
    {
        ResourceName = typeof(T).Name;
    }
}