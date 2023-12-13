using AutoMapper;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Entities;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Exceptions;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.SeedWorks.Specifications;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

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
        var specification = await GetSpecificationAsync(request);
        var activities = await _eventActivityRepository.FilterAsync(specification);

        return _mapper.Map<IEnumerable<EventActivityDto>>(activities.OrderBy(x => x.Index));
    }

    private async Task<ISpecification<EventActivity, Guid>> GetSpecificationAsync(GetAllEventActivityQuery request)
    {
        var specification = EmptySpecification<EventActivity, Guid>.Instance;
        if (request.EventCategoryId.HasValue)
        {
            if (!await _eventCategoryRepository.IsExistingAsync(request.EventCategoryId.Value))
            {
                throw new EventCategoryNotFoundException(request.EventCategoryId.Value);
            }

            specification = specification.And(new EventActivityByCategorySpecification(request.EventCategoryId.Value));
        }

        if (request.Type.HasValue)
        {
            specification = specification.And(new EventActivityByTypeSpecification(request.Type.Value));
        }

        return specification;
    }
}