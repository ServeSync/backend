namespace ServeSync.Application.SeedWorks.Schedulers;

public interface IBackGroundJobPublisher
{
    void Publish(IBackGroundJob job);
}