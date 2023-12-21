using Microsoft.EntityFrameworkCore;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.Events;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

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
        
        return EnrichResult(result, request);;
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
                        Key = x.Key.Month,
                        SubKey = x.Key.Year
                    });
            
            case TimeType.Year:
                return queryable.GroupBy(x => x.Created.Year)
                    .Select(x => new EventStudentStatisticDto()
                    {
                        Name = x.Key.ToString(),
                        Value = x.Count(),
                        Key = x.Key,
                    });
            
            default:
                return queryable.GroupBy(x => x.Created.Date)
                    .Select(x => new EventStudentStatisticDto()
                    {
                        Name = x.Key.ToString("dd/MM/yyyy"),
                        Value = x.Count(),
                        Key = x.Key.Day
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
            
            case TimeType.Week:
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
    
    private List<EventStudentStatisticDto> EnrichResult(List<EventStudentStatisticDto> result, GetEventRegisteredStudentStatisticQuery request)
    {
        var dateTime = DateTime.UtcNow;
        
        switch (request.TimeType)
        {
            case TimeType.Date:
                result.AddRange(Enumerable.Range(0, request.NumberOfRecords)
                    .Select(offset => dateTime.AddDays(-offset))
                    .Where(dt => result.All(x => x.Key != dt.Day))
                    .Select(dt => new EventStudentStatisticDto
                    {
                        Name = dt.ToString("dd/MM/yyyy"),
                        Value = 0,
                        Key = dt.Day
                    }));
                break;
            
            case TimeType.Week:
                break;
            
            case TimeType.Month:
                result.AddRange(Enumerable.Range(0, request.NumberOfRecords)
                    .Select(offset => dateTime.AddMonths(-offset))
                    .Where(dt => result.All(x => x.Key != dt.Month || x.SubKey != dt.Year))
                    .Select(dt => new EventStudentStatisticDto
                    {
                        Name = $"{dt.Month}/{dt.Year}",
                        Value = 0,
                        Key = dt.Month,
                        SubKey = dt.Year
                    }).ToList());
                break;
            
            case TimeType.Year:
                result.AddRange(Enumerable.Range(0, request.NumberOfRecords)
                    .Select(offset => dateTime.AddYears(-offset))
                    .Where(dt => result.All(x => x.Key != dt.Year))
                    .Select(dt => new EventStudentStatisticDto
                    {
                        Name = dt.Year.ToString(),
                        Value = 0,
                        Key = dt.Year
                    }));
                break;
        }

        return result.OrderBy(x => x.SubKey).ThenBy(x => x.Key).ToList();
    }
}