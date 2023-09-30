using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.SeedWorks.Exceptions;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Specifications;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Exceptions;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Specifications;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Jobs;

public class ImportStudentFromCsvBackGroundJobHandler : IBackGroundJobHandler<ImportStudentFromCsvBackGroundJob>
{
    private readonly IStudentDomainService _studentDomainService;
    private readonly IStudentRepository _studentRepository;
    private readonly IHomeRoomRepository _homeRoomRepository;
    private readonly IEducationProgramRepository _educationProgramRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ImportStudentFromCsvBackGroundJobHandler> _logger;
    
    public ImportStudentFromCsvBackGroundJobHandler(
        IStudentDomainService studentDomainService,
        IStudentRepository studentRepository,
        IHomeRoomRepository homeRoomRepository,
        IEducationProgramRepository educationProgramRepository,
        IUnitOfWork unitOfWork,
        ILogger<ImportStudentFromCsvBackGroundJobHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _studentRepository = studentRepository;
        _homeRoomRepository = homeRoomRepository;
        _educationProgramRepository = educationProgramRepository;
        _studentDomainService = studentDomainService;
    }
    
    public async Task Handle(ImportStudentFromCsvBackGroundJob job, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        var homeRoomIdByName = await GetIncludedHomeRooms(job);
        var educationProgramIdByName = await GetIncludedEducationPrograms(job);

        foreach (var student in job.Students)
        {
            try
            {
                if (!homeRoomIdByName.ContainsKey(student.HomeRoom))
                {
                    throw new HomeRoomNotFoundException(student.HomeRoom);
                }
                
                if (!educationProgramIdByName.ContainsKey(student.EducationProgram))
                {
                    throw new EducationProgramNotFoundException(student.EducationProgram);
                }
                
                var createdStudent = await _studentDomainService.CreateAsync(
                    student.Code,
                    student.FullName,
                    student.Gender,
                    student.Birth,
                    student.ImageUrl,
                    student.CitizenId,
                    student.Email,
                    student.Phone,
                    homeRoomIdByName[student.HomeRoom],
                    educationProgramIdByName[student.EducationProgram],
                    student.HomeTown,
                    student.Address,
                    autoSave: false);

                await _studentRepository.InsertAsync(createdStudent);
            }
            catch (CoreException e)
            {
                _logger.LogError("Import student failed due to business failure: {Message}", e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Import student failed: {Message}", e.Message);
            }
        }
    
        await _unitOfWork.CommitTransactionAsync(true);
    }
    
    private async Task<IDictionary<string, Guid>> GetIncludedHomeRooms(ImportStudentFromCsvBackGroundJob job)
    {
        var homeRoomNames = job.Students.Select(x => x.HomeRoom).Distinct();
        var homeRooms = await _homeRoomRepository.FilterAsync(new HomeRoomsByIncludedNamesSpecification(homeRoomNames));

        return homeRooms.ToDictionary(homeRoom => homeRoom.Name, homeRoom => homeRoom.Id);
    }

    private async Task<IDictionary<string, Guid>> GetIncludedEducationPrograms(ImportStudentFromCsvBackGroundJob job)
    {
        var educationProgramNames = job.Students.Select(x => x.EducationProgram).Distinct();
        var educationPrograms = await _educationProgramRepository.FilterAsync(new EducationProgramsByIncludedNamesSpecification(educationProgramNames));

        return educationPrograms.ToDictionary(educationProgram => educationProgram.Name, educationProgram => educationProgram.Id);
    }
}