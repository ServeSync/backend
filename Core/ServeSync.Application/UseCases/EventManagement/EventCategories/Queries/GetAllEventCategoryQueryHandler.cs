using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.UseCases.EventManagement.EventCategories.Queries;
public class GetAllEventCategoryQueryHandler : IQueryHandler<GetAllEventCategoryQuery, IEnumerable<EventCategoryDto>>
{
    private readonly IBasicReadOnlyRepository<EventCategory, Guid> _eventCategoryRepository;
    private readonly IMapper _mapper;

    public GetAllEventCategoryQueryHandler(IBasicReadOnlyRepository<EventCategory, Guid> eventCategoryRepository, IMapper mapper)
    {
        _eventCategoryRepository = eventCategoryRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<EventCategoryDto>> Handle(GetAllEventCategoryQuery request, CancellationToken cancellationToken)
    {
        var specification = GetSpecification(request);
        var eventCategories = await _eventCategoryRepository.FilterAsync(specification);
        return _mapper.Map<IEnumerable<EventCategoryDto>>(eventCategories.OrderBy(x => x.Index));
    }
    
    private ISpecification<EventCategory, Guid> GetSpecification(GetAllEventCategoryQuery request)
    {
        var specification = EmptySpecification<EventCategory, Guid>.Instance
            .AndIf(request.Type.HasValue, new EventCategoryByTypeSpecification(request.Type.GetValueOrDefault()));
     
        return specification;
    }
}