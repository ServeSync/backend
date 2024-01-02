using Microsoft.EntityFrameworkCore;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetEventAttendanceStudentStatisticQueryHandler : IQueryHandler<GetEventAttendanceStudentStatisticQuery, List<EventStudentStatisticDto>>
{
    private readonly IStudentRepository _studentRepository;
    
    public GetEventAttendanceStudentStatisticQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    public async Task<List<EventStudentStatisticDto>> Handle(GetEventAttendanceStudentStatisticQuery request, CancellationToken cancellationToken)
    {
        var queryable = await GetQueryableAsync(request);
        var result =  await queryable.ToListAsync(cancellationToken);
        
        return StatisticHelper.EnrichResult(result, request.TimeType, request.NumberOfRecords);
    }

    private async Task<IQueryable<EventStudentStatisticDto>> GetQueryableAsync(GetEventAttendanceStudentStatisticQuery request)
    {
        var specification = GetSpecification(request);
        var queryable = await _studentRepository.GetEventStudentAttendanceQueryable(specification);

        switch (request.TimeType)
        {
            case TimeType.Month:
                return queryable.GroupBy(x => new { x.AttendanceAt.Month, x.AttendanceAt.Year })
                    .Select(x => new EventStudentStatisticDto()
                    {
                        Name = $"{x.Key.Month}/{x.Key.Year}",
                        Value = x.Count(),
                        Month = x.Key.Month,
                        Year = x.Key.Year
                    });
            
            case TimeType.Year:
                return queryable.GroupBy(x => x.AttendanceAt.Year)
                    .Select(x => new EventStudentStatisticDto()
                    {
                        Name = x.Key.ToString(),
                        Value = x.Count(),
                        Year = x.Key,
                    });
            
            default:
                return queryable.GroupBy(x => x.AttendanceAt.Date)
                    .Select(x => new EventStudentStatisticDto()
                    {
                        Name = x.Key.ToString("dd/MM/yyyy"),
                        Value = x.Count(),
                        Day = x.Key.Day,
                        Month = x.Key.Month,
                        Year = x.Key.Year
                    });
        }
    }
    
    private ISpecification<StudentEventAttendance, Guid> GetSpecification(GetEventAttendanceStudentStatisticQuery request)
    {
        var dateTime = DateTime.UtcNow.CurrentTimeZone();
        
        switch (request.TimeType)
        {
            case TimeType.Date:
                return new EventAttendanceByTimeFrameSpecification(dateTime.AddDays(-request.NumberOfRecords), dateTime);
            
            case TimeType.Month:
                return new EventAttendanceByTimeFrameSpecification(dateTime.AddMonths(-request.NumberOfRecords), dateTime);
            
            case TimeType.Year:
                return new EventAttendanceByTimeFrameSpecification(dateTime.AddYears(-request.NumberOfRecords), dateTime);
        }
        
        return EmptySpecification<StudentEventAttendance, Guid>.Instance;
    }
}