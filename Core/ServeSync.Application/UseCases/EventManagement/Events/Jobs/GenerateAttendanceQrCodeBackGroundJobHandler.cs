using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.ImageUploader;
using ServeSync.Application.SeedWorks.Schedulers;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class GenerateAttendanceQrCodeBackGroundJobHandler : IBackGroundJobHandler<GenerateAttendanceQrCodeBackGroundJob>
{
    private readonly IServer _server;
    private readonly IImageUploader _imageUploader;
    private readonly ILogger<GenerateAttendanceQrCodeBackGroundJob> _logger;
    
    public GenerateAttendanceQrCodeBackGroundJobHandler(
        IServer server,
        IImageUploader imageUploader,
        ILogger<GenerateAttendanceQrCodeBackGroundJob> logger)
    {
        _server = server;
        _imageUploader = imageUploader;
        _logger = logger;
    }
    
    public Task Handle(GenerateAttendanceQrCodeBackGroundJob job, CancellationToken cancellationToken)
    {
        var baseUrl = _server.Features.Get<IServerAddressesFeature>().Addresses.First();
        foreach (var attendance in job.AttendanceInfos)
        {
            var attendanceUrl = $"{baseUrl}/api/event-attendances/{attendance.Id}";
            
            var callBackUrlWithToken = QueryHelpers.AddQueryString(attendanceUrl, new Dictionary<string, string>()
            {
                {"code", attendance.Code }
            });
            
            var qrCode = QrCodeGenerator.GeneratePng(callBackUrlWithToken);
            _imageUploader.PushUpload($"event-attendances-{attendance.Id}", qrCode);
        }

        return Task.CompletedTask;
    }
}