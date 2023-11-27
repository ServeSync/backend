using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServeSync.Application.SeedWorks.Schedulers;

public class BackGroundJobPublisher : IBackGroundJobPublisher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackGroundJobPublisher> _logger;
    
    public BackGroundJobPublisher(IServiceProvider serviceProvider, ILogger<BackGroundJobPublisher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    
    public async Task Publish(IBackGroundJob job)
    {
        var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        _logger.LogInformation("Job {JobName} published!", job.GetType().Name);
        try
        {
            await mediator.Publish(job);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while publishing job {JobName}", job.GetType().Name);
        }
    }
}