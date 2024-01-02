using Microsoft.EntityFrameworkCore;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetEventRegisteredStudentStatisticQueryHandler : IQueryHandler<GetEventRegisteredStudentStatisticQuery, List<EventStudentStatisticDto>>
{
    private readonly IStudentRepository _studentRepository;
    
    public GetEventRegisteredStudentStatisticQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    public async Task<List<EventStudentStatisticDto>> Handle(GetEventRegisteredStudentStatisticQuery request, CancellationToken cancellationToken)
    {
        var queryable = await GetQueryableAsync(request);
        var result =  await queryable.ToListAsync(cancellationToken);
        
        return StatisticHelper.EnrichResult(result, request.TimeType, request.NumberOfRecords);
    }

    private async Task<IQueryable<EventStudentStatisticDto>> GetQueryableAsync(GetEventRegisteredStudentStatisticQuery request)
    {
        var specification = GetSpecification(request);
        var queryable = await _studentRepository.GetEventStudentRegisteredQueryable(specification);

        switch (request.TimeType)
        {
            case TimeType.Month:
                return queryable.GroupBy(x => new { x.Created.Month, x.Created.Year })
                    .Select(x => new EventStudentStatisticDto()
                    {
                        Name = $"{x.Key.Month}/{x.Key.Year}",
                        Value = x.Count(),
                        Month = x.Key.Month,
                        Year = x.Key.Year
                    });
            
            case TimeType.Year:
                return queryable.GroupBy(x => x.Created.Year)
                    .Select(x => new EventStudentStatisticDto()
                    {
                        Name = x.Key.ToString(),
                        Value = x.Count(),
                        Year = x.Key,
                    });
            
            default:
                return queryable.GroupBy(x => x.Created.Date)
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
    
    private ISpecification<StudentEventRegister, Guid> GetSpecification(GetEventRegisteredStudentStatisticQuery request)
    {
        var dateTime = DateTime.UtcNow;
        ISpecification<StudentEventRegister, Guid> specification = new EventRegisterByStatusSpecification(EventRegisterStatus.Approved);

        switch (request.TimeType)
        {
            case TimeType.Date:
                specification = specification.And(new EventRegisterByTimeFrameSpecification(dateTime.AddDays(-request.NumberOfRecords), dateTime));
                break;
            
            case TimeType.Month:
                specification = specification.And(new EventRegisterByTimeFrameSpecification(dateTime.AddMonths(-request.NumberOfRecords), dateTime));
                break;
            
            case TimeType.Year:
                specification = specification.And(new EventRegisterByTimeFrameSpecification(dateTime.AddYears(-request.NumberOfRecords), dateTime));
                break;
        }
        
        return specification;
    }
}