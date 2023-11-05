using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;

public class GetAllEventActivityQueryHandler : IQueryHandler<GetAllEventActivityQuery, IEnumerable<EventActivityDto>>
{
    private readonly IBasicReadOnlyRepository<EventCategory, Guid> _eventCategoryRepository;
    private readonly IBasicReadOnlyRepository<EventActivity, Guid> _eventActivityRepository;
    private readonly IMapper _mapper;
    
    public GetAllEventActivityQueryHandler(
        IBasicReadOnlyRepository<EventCategory, Guid> eventCategoryRepository,
        IBasicReadOnlyRepository<EventActivity, Guid> eventActivityRepository,
        IMapper mapper)
    {
        _eventCategoryRepository = eventCategoryRepository;
        _eventActivityRepository = eventActivityRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EventActivityDto>> Handle(GetAllEventActivityQuery request, CancellationToken cancellationToken)
    {
        if (!await _eventCategoryRepository.IsExistingAsync(request.EventCategoryId))
        {
            throw new EventCategoryNotFoundException(request.EventCategoryId);
        }

        var specification = new EventActivityByCategorySpecification(request.EventCategoryId);
        var activities = await _eventActivityRepository.FilterAsync(specification);

        return _mapper.Map<IEnumerable<EventActivityDto>>(activities.OrderBy(x => x.Index));
    }
}