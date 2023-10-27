using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.StudentInEvents;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredStudentsQueryHandler : IQueryHandler<GetAllRegisteredStudentsQuery, PagedResultDto<RegisteredStudentInEventDto>>
{
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly IMapper _mapper;
    public GetAllRegisteredStudentsQueryHandler(
        IMapper mapper,
        IEventReadModelRepository eventReadModelRepository)
    {
        _mapper = mapper;
        _eventReadModelRepository = eventReadModelRepository;
    }
    
    public async Task<PagedResultDto<RegisteredStudentInEventDto>> Handle(GetAllRegisteredStudentsQuery request, CancellationToken cancellationToken)
    {
        var (registeredStudents, total) = await _eventReadModelRepository.GetPagedRegisteredStudentsInEventAsync(request.EventId, request.Page, request.Size);
        if (registeredStudents == null)
        {
            throw new EventNotFoundException(request.EventId);
        }

        return new PagedResultDto<RegisteredStudentInEventDto>(
            total!.Value,
            request.Size,
            _mapper.Map<List<RegisteredStudentInEventDto>>(registeredStudents)
        );
    }
}