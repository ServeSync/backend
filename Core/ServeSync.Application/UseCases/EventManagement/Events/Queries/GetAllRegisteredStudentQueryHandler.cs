using AutoMapper;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.UseCases.EventManagement.Events.Dtos.EventRoles;
using ServeSync.Domain.EventManagement.EventAggregate.Exceptions;

namespace ServeSync.Application.UseCases.EventManagement.Events.Queries;

public class GetAllRegisteredStudentQueryHandler : IQueryHandler<GetAllRegisteredStudentQuery, PagedResultDto<RegisteredStudentInEventDto>>
{
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly IMapper _mapper;
    public GetAllRegisteredStudentQueryHandler(
        IMapper mapper,
        IEventReadModelRepository eventReadModelRepository)
    {
        _mapper = mapper;
        _eventReadModelRepository = eventReadModelRepository;
    }
    
    public async Task<PagedResultDto<RegisteredStudentInEventDto>> Handle(GetAllRegisteredStudentQuery request, CancellationToken cancellationToken)
    {
        var (registeredStudents, total) = await _eventReadModelRepository.GetPagedRegisteredStudentsInEventRoleAsync(request.EventRoleId, request.Page, request.Size);
        if (registeredStudents == null)
        {
            throw new EventRoleNotFoundException(request.EventRoleId);
        }

        return new PagedResultDto<RegisteredStudentInEventDto>(
            total!.Value,
            request.Size,
            _mapper.Map<List<RegisteredStudentInEventDto>>(registeredStudents)
        );
    }
}