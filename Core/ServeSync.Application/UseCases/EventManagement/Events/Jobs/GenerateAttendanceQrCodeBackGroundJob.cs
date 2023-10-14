using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;

namespace ServeSync.Application.UseCases.EventManagement.Events.Jobs;

public class GenerateAttendanceQrCodeBackGroundJob : IBackGroundJob
{
    public IEnumerable<AttendanceInfoGenerateQrCodeBackGroundJobDto> AttendanceInfos { get; set; }
    
    public GenerateAttendanceQrCodeBackGroundJob(IEnumerable<AttendanceInfoGenerateQrCodeBackGroundJobDto> attendanceInfos)
    {
        AttendanceInfos = attendanceInfos;
    }
}

public class AttendanceInfoGenerateQrCodeBackGroundJobDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
}