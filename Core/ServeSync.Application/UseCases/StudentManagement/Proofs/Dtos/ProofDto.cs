using System.Linq.Expressions;
using ServeSync.Application.UseCases.EventManagement.EventCategories.Dtos;
using ServeSync.Application.UseCases.StudentManagement.Students.Dtos;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Enums;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class ProofDto : IProjection<Proof, Guid, ProofDto>
{
    public Guid Id { get; set; }
    public ProofStatus ProofStatus { get; set; }
    public ProofType ProofType { get; set; }
    
    public string EventName { get; set; } = null!;
    public string OrganizationName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public double Score { get; set; }
    
    public SimpleStudentDto Student { get; set; } = null!;
    public EventActivityDto Activity { get; set; } = null!;
    
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    
    public Expression<Func<Proof, ProofDto>> GetProject()
    {
        return x => new ProofDto()
        {
            Id = x.Id,
            ProofStatus = x.ProofStatus,
            ProofType = x.ProofType,
            EventName = x.ProofType == ProofType.Internal 
                ? x.InternalProof!.Event!.Name 
                : x.ExternalProof!.EventName,
            OrganizationName = x.ProofType == ProofType.Internal 
                ? x.InternalProof!.Event!.RepresentativeOrganization!.Organization!.Name 
                : x.ExternalProof!.OrganizationName,
            Role = x.ProofType == ProofType.Internal
                ? x.InternalProof!.EventRole!.Name
                : x.ExternalProof!.Role,
            Address = x.ProofType == ProofType.Internal
                ? x.InternalProof!.Event!.Address.FullAddress
                : x.ExternalProof!.Address,
            ImageUrl = x.ImageUrl,
            Score = x.ProofType == ProofType.Internal
                ? x.InternalProof!.EventRole!.Score
                : x.ExternalProof!.Score,
            Student = new SimpleStudentDto()
            {
                Id = x.Student!.Id,
                FullName = x.Student.FullName,
                Email = x.Student.Email,
                ImageUrl = x.Student.ImageUrl,
            },
            Activity = new EventActivityDto()
            {
                Id = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.ActivityId
                    : x.ExternalProof!.ActivityId,
                Name = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.Name
                    : x.ExternalProof!.Activity!.Name,
                MinScore = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.MinScore
                    : x.ExternalProof!.Activity!.MinScore,
                MaxScore = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.MaxScore
                    : x.ExternalProof!.Activity!.MaxScore,
                EventCategoryId = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.EventCategoryId
                    : x.ExternalProof!.Activity!.EventCategoryId,
            },
            StartAt = x.ProofType == ProofType.Internal
                ? x.InternalProof!.Event!.StartAt
                : x.ExternalProof!.StartAt,
            EndAt = x.ProofType == ProofType.Internal
                ? x.InternalProof!.Event!.EndAt
                : x.ExternalProof!.EndAt,
            Created = x.Created,
            LastModified = x.LastModified,
        };
    }
}

public class ProofDetailDto : ProofDto, IProjection<Proof, Guid, ProofDetailDto>
{
    public string? Description { get; set; }
    public string? RejectReason { get; set; }
    
    public Expression<Func<Proof, ProofDetailDto>> GetProject()
    {
        return x => new ProofDetailDto()
        {
            Id = x.Id,
            ProofStatus = x.ProofStatus,
            ProofType = x.ProofType,
            EventName = x.ProofType == ProofType.Internal 
                ? x.InternalProof!.Event!.Name 
                : x.ExternalProof!.EventName,
            OrganizationName = x.ProofType == ProofType.Internal 
                ? x.InternalProof!.Event!.RepresentativeOrganization!.Organization!.Name 
                : x.ExternalProof!.OrganizationName,
            Role = x.ProofType == ProofType.Internal
                ? x.InternalProof!.EventRole!.Name
                : x.ExternalProof!.Role,
            Address = x.ProofType == ProofType.Internal
                ? x.InternalProof!.Event!.Address.FullAddress
                : x.ExternalProof!.Address,
            ImageUrl = x.ImageUrl,
            Score = x.ProofType == ProofType.Internal
                ? x.InternalProof!.EventRole!.Score
                : x.ExternalProof!.Score,
            Student = new SimpleStudentDto()
            {
                Id = x.Student!.Id,
                FullName = x.Student.FullName,
                Email = x.Student.Email,
                ImageUrl = x.Student.ImageUrl,
            },
            Activity = new EventActivityDto()
            {
                Id = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.ActivityId
                    : x.ExternalProof!.ActivityId,
                Name = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.Name
                    : x.ExternalProof!.Activity!.Name,
                MinScore = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.MinScore
                    : x.ExternalProof!.Activity!.MinScore,
                MaxScore = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.MaxScore
                    : x.ExternalProof!.Activity!.MaxScore,
                EventCategoryId = x.ProofType == ProofType.Internal
                    ? x.InternalProof!.Event!.Activity!.EventCategoryId
                    : x.ExternalProof!.Activity!.EventCategoryId,
            },
            StartAt = x.ProofType == ProofType.Internal
                ? x.InternalProof!.Event!.StartAt
                : x.ExternalProof!.StartAt,
            EndAt = x.ProofType == ProofType.Internal
                ? x.InternalProof!.Event!.EndAt
                : x.ExternalProof!.EndAt,
            Created = x.Created,
            LastModified = x.LastModified,
            Description = x.Description,
            RejectReason = x.RejectReason
        };
    }
}