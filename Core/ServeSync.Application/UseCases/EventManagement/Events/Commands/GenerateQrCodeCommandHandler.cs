using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.ImageUploader;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.UseCases.EventManagement.Events.Jobs;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;

namespace ServeSync.Application.UseCases.EventManagement.Events.Commands;

public class GenerateQrCodeCommandHandler : ICommandHandler<GenerateQrCodeCommand>
{
    private readonly IEventDomainService _eventDomainService;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploader _imageUploader;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GenerateAttendanceQrCodeBackGroundJob> _logger;
    
    public GenerateQrCodeCommandHandler(
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
    
    public async Task Handle(GenerateQrCodeCommand request, CancellationToken cancellationToken)
    {
        var events = request.Ids.Length == 0
            ? await _eventRepository.FindAllAsync()
            : await _eventRepository.FindByIncludedIdsAsync(request.Ids);
        
        var attendanceUrl = _configuration["Urls:Web:AttendanceUrl"];
        foreach (var @event in events)
        {
            foreach (var attendance in @event.AttendanceInfos.Where(x => string.IsNullOrEmpty(x.QrCodeUrl)))
            {
                var callBackUrlWithToken = QueryHelpers.AddQueryString(attendanceUrl, new Dictionary<string, string>()
                {
                    {"Code", attendance.Code },
                    {"EventId", @event.Id.ToString() }
                });
            
                var qrCode = QrCodeGenerator.GeneratePng(callBackUrlWithToken);
                var imageUrl = await _imageUploader.UploadAsync($"event-attendances-{attendance.Id}", qrCode);

                _eventDomainService.SetAttendanceQrCodeUrl(@event, attendance.Id, imageUrl.Url!);
            }
        
            _eventRepository.Update(@event);   
        }
        
        await _unitOfWork.CommitAsync();
    }
}