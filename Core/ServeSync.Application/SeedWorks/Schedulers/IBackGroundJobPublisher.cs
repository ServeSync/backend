namespace ServeSync.Application.SeedWorks.Schedulers;

public interface IBackGroundJobPublisher
{
    Task Publish(IBackGroundJob job);
}