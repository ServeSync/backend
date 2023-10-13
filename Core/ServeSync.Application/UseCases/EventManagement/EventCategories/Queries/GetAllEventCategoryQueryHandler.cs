using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;
public class GetAllEventCategoryQueryHandler : IQueryHandler<GetAllEventCategoryQuery, PagedResultDto<EventCategoryDto>>
{
    private readonly IBasicReadOnlyRepository<EventCategory, Guid> _eventCategoryRepository;
    private IMapper _mapper;

    public GetAllEventCategoryQueryHandler(IBasicReadOnlyRepository<EventCategory, Guid> eventCategoryRepository, IMapper mapper)
    {
        _eventCategoryRepository = eventCategoryRepository;
        _mapper = mapper;
    }
    public async Task<PagedResultDto<EventCategoryDto>> Handle(GetAllEventCategoryQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSpecification(request);

        var categories = await _eventCategoryRepository.GetPagedListAsync(specification);
        var total = await _eventCategoryRepository.GetCountAsync(specification);

        return new PagedResultDto<EventCategoryDto>(
            total, request.Size,
            _mapper.Map<IEnumerable<EventCategoryDto>>(categories)
        );
    }

    private IPagingAndSortingSpecification<EventCategory, Guid> GetSpecification(GetAllEventCategoryQuery request)
    {
        var specification = new FilterEventCategorySpecification(request.Page, request.Size, request.Sorting, request.Search);

        return specification;
    }
}