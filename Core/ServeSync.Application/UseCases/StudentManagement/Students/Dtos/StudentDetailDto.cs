using System.Linq.Expressions;
using ServeSync.Application.UseCases.StudentManagement.EducationPrograms.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Faculties.Dtos;
using ServeSync.Application.UseCases.StudentManagement.HomeRooms.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Application.UseCases.StudentManagement.Students.Dtos;

public class StudentDetailDto : BasicStudentDto, IProjection<Student, Guid, StudentDetailDto>
{
    public double Score { get; set; }
    public FacultyDto Faculty { get; set; } = null!;
    public HomeRoomDto HomeRoom { get; set; } = null!;
    public EducationProgramDto EducationProgram { get; set; } = null!;


    public Expression<Func<Student, StudentDetailDto>> GetProject()
    {
        return x => new StudentDetailDto()
        {
            Id = x.Id,
            Code = x.Code,
            FullName = x.FullName,
            Gender = x.Gender,
            Address = x.Address,
            CitizenId = x.CitizenId,
            DateOfBirth = x.DateOfBirth,
            HomeTown = x.HomeTown,
            ImageUrl = x.ImageUrl,
            Email = x.Email,
            Phone = x.Phone,
            IdentityId = x.IdentityId,
            Score = x.EventRegisters.Sum(y => y.StudentEventAttendance != null ? y.EventRole!.Score : 0)
                    + x.Proofs.Where(y => y.ProofStatus == ProofStatus.Approved).Sum(y => y.ProofType == ProofType.Internal 
                        ? y.InternalProof!.EventRole!.Score
                        : y.ProofType == ProofType.External
                            ? y.ExternalProof!.Score
                            : y.SpecialProof!.Score),
            Faculty = new FacultyDto()
            {
                Id = x.HomeRoom!.Faculty!.Id,
                Name = x.HomeRoom.Faculty.Name,
            },
            HomeRoom = new HomeRoomDto()
            {
                Id = x.HomeRoom!.Id,
                Name = x.HomeRoom.Name,
                FacultyId = x.HomeRoom.FacultyId
            },
            EducationProgram = new EducationProgramDto()
            {
                Id = x.EducationProgram!.Id,
                Name = x.EducationProgram.Name,
                RequiredCredit = x.EducationProgram.RequiredCredit,
                RequiredActivityScore = x.EducationProgram.RequiredActivityScore,
            }
        };
    }
}