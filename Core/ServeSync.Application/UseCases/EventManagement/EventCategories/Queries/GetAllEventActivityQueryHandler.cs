using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;

public class GetAllEventActivityQueryHandler : IQueryHandler<GetAllEventActivityQuery, PagedResultDto<EventActivityDto>>
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
    
    public async Task<PagedResultDto<EventActivityDto>> Handle(GetAllEventActivityQuery request, CancellationToken cancellationToken)
    {
        if (!await _eventCategoryRepository.IsExistingAsync(request.EventCategoryId))
        {
            throw new EventCategoryNotFoundException(request.EventCategoryId);
        }

        var specification = GetSpecification(request);
        var activities = await _eventActivityRepository.GetPagedListAsync(specification);
        var total = await _eventActivityRepository.GetCountAsync(specification);

        return new PagedResultDto<EventActivityDto>(
            total,
            request.Size,
            _mapper.Map<IEnumerable<EventActivityDto>>(activities));
    }

    private IPagingAndSortingSpecification<EventActivity, Guid> GetSpecification(GetAllEventActivityQuery request)
    {
        var specification = new FilterEventActivitySpecification(request.Page, request.Size, request.Sorting, request.Search)
            .And(new EventActivityByCategorySpecification(request.EventCategoryId));

        return specification;
    }
}