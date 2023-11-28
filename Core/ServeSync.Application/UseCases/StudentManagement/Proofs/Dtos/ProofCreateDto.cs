namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class ProofCreateDto
{
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = null!;
    public DateTime AttendanceAt { get; set; }
}

public class InternalProofCreateDto : ProofCreateDto
{
    public Guid EventId { get; set; }
    public Guid EventRoleId { get; set; }
}