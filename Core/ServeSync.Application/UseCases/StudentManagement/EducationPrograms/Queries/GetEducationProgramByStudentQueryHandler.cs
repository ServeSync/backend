using AutoMapper;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.ProofAggregate.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Queries;

public class GetEducationProgramByStudentQueryHandler : IQueryHandler<GetEducationProgramByStudentQuery, StudentEducationProgramDto>
{
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    private readonly IEducationCachingManager _educationCachingManager;
    private readonly IMapper _mapper;
    private readonly IEventReadModelRepository _eventReadModelRepository;
    private readonly IProofRepository _proofRepository;
    
    public GetEducationProgramByStudentQueryHandler(
        IBasicReadOnlyRepository<Student, Guid> studentRepository,
        IEducationCachingManager educationCachingManager,
        IMapper mapper,
        IEventReadModelRepository eventReadModelRepository,
        IProofRepository proofRepository)
    {
        _studentRepository = studentRepository;
        _educationCachingManager = educationCachingManager;
        _mapper = mapper;
        _eventReadModelRepository = eventReadModelRepository;
        _proofRepository = proofRepository;
    } 
    
    public async Task<StudentEducationProgramDto> Handle(GetEducationProgramByStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.FindByIdAsync(request.StudentId);
        if (student == null)
        {
            throw new StudentNotFoundException(request.StudentId);
        }

        var educationProgram = _mapper.Map<StudentEducationProgramDto>(await _educationCachingManager.GetByIdAsync(student.EducationProgramId));
        
        educationProgram.NumberOfEvents = await _eventReadModelRepository.GetCountNumberOfAttendedEventsOfStudentAsync(request.StudentId);
        educationProgram.EventScore = await _eventReadModelRepository.GetSumScoreOfAttendedEventsOfStudentAsync(request.StudentId);
        educationProgram.ProofScore = await _proofRepository.GetSumScoreOfStudentAsync(request.StudentId);
        educationProgram.NumberOfProofs = await _proofRepository.GetCountAsync(new ProofByStudentIdSpecification(student.Id));
        educationProgram.NumberOfApprovedProofs = await _proofRepository.GetCountAsync(new ProofByStudentIdSpecification(student.Id)
            .And(new ProofByStatusSpecification(ProofStatus.Approved)));
        
        return educationProgram;
    }
}