using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Helpers;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Application.UseCases.Statistics.Dtos;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.EventManagement.EventAggregate.Enums;
using ServeSync.Domain.EventManagement.EventAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.Statistics.Queries;

public class GetEventStatisticQueryHandler : IQueryHandler<GetEventStatisticQuery, EventStatisticDto>
{
    private readonly IReadOnlyRepository<Event, Guid> _eventRepository;
    private readonly ISpecificationService _specificationService;
    
    public GetEventStatisticQueryHandler(IReadOnlyRepository<Event, Guid> eventRepository,
        ISpecificationService specificationService)
    {
        _eventRepository = eventRepository;
        _specificationService = specificationService;
    }
    
    public async Task<EventStatisticDto> Handle(GetEventStatisticQuery request, CancellationToken cancellationToken)
    {
        var statisticStatuses = new List<EventStatus>()
        {
            EventStatus.Rejected,
            EventStatus.Cancelled,
            EventStatus.Happening,
            EventStatus.Done,
            EventStatus.Expired,
            EventStatus.Upcoming,
            EventStatus.Pending
        };
        
        var dateTime = DateTime.UtcNow.CurrentTimeZone();
        var specification = (await _specificationService.GetEventPersonalizedSpecificationAsync())
            .And(GetSpecificationByRecurringType(request.Type, dateTime, request.StartAt, request.EndAt));
        
        var result = new EventStatisticDto()
        {
            Total = await _eventRepository.GetCountAsync(specification),
            Data = new List<EventStatisticRecordDto>()
        };
        
        foreach (var status in statisticStatuses)
        {
            var eventStatusSpecification = specification.And(new EventByStatusSpecification(status, dateTime));

            result.Data.Add(new EventStatisticRecordDto()
            {
                Status = status,
                Count = await _eventRepository.GetCountAsync(eventStatusSpecification)
            });
        }
        
        return result;
    }
    
    private ISpecification<Event, Guid> GetSpecificationByRecurringType(RecurringFilterType? type, 
        DateTime dateTime,
        DateTime? startAt,
        DateTime? endAt)
    {
        if (!type.HasValue)
        {
            return EmptySpecification<Event, Guid>.Instance;
        }
        
        if (type is not RecurringFilterType.Custom)
            (startAt, endAt) = DateTimeHelper.GetByRecurringType(type.Value, dateTime);
        
        return new EventByTimeFrameSpecification(startAt, endAt);
    }
}