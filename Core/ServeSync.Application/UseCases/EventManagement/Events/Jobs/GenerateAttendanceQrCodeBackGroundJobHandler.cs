using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.ImageUploader;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class GenerateAttendanceQrCodeBackGroundJobHandler : IBackGroundJobHandler<GenerateAttendanceQrCodeBackGroundJob>
{
    private readonly IEventDomainService _eventDomainService;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploader _imageUploader;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GenerateAttendanceQrCodeBackGroundJob> _logger;
    
    public GenerateAttendanceQrCodeBackGroundJobHandler(
        IEventDomainService eventDomainService,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IImageUploader imageUploader,
        ILogger<GenerateAttendanceQrCodeBackGroundJob> logger)
    {
        _eventDomainService = eventDomainService;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _imageUploader = imageUploader;
        _logger = logger;
    }
    
    public async Task Handle(GenerateAttendanceQrCodeBackGroundJob job, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.FindByIdAsync(job.EventId);
        if (@event == null)
        {
            _logger.LogError("Event with id {EventId} not found", job.EventId);
            return;
        }
        
        var attendanceUrl = _configuration["Urls:Web:AttendanceUrl"];
        foreach (var attendance in @event.AttendanceInfos)
        {
            var callBackUrlWithToken = QueryHelpers.AddQueryString(attendanceUrl, new Dictionary<string, string>()
            {
                {"Code", attendance.Code },
                {"EventId", job.EventId.ToString() }
            });
            
            var qrCode = QrCodeGenerator.GeneratePng(callBackUrlWithToken);
            var imageUrl = await _imageUploader.UploadAsync($"event-attendances-{attendance.Id}", qrCode);

            _eventDomainService.SetAttendanceQrCodeUrl(@event, attendance.Id, imageUrl.Url!);
        }
        
        _eventRepository.Update(@event);
        await _unitOfWork.CommitAsync();
    }
}