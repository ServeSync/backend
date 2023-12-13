namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class ProofUpdateDto
{
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = null!;
}

public class InternalProofUpdateDto : ProofUpdateDto
{
    public Guid EventId { get; set; }
    public Guid EventRoleId { get; set; }
    public DateTime AttendanceAt { get; set; }
}

public class ExternalProofUpdateDto : ProofUpdateDto
{
    public string EventName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string OrganizationName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public double Score { get; set; }
    
    public DateTime? AttendanceAt { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    
    public Guid ActivityId { get; set; }
}

public class SpecialProofUpdateDto : ProofUpdateDto
{
    public string Title { get; set; } = null!;
    
    public string Role { get; set; } = null!;
    public double Score { get; set; }
    
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    
    public Guid ActivityId { get; set; }
}