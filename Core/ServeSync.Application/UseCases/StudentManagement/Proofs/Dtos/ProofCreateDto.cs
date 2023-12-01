﻿namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class ProofCreateDto
{
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = null!;
    public DateTime AttendanceAt { get; set; }
    public string? RejectReason { get; set; }
}

public class InternalProofCreateDto : ProofCreateDto
{
    public Guid EventId { get; set; }
    public Guid EventRoleId { get; set; }
}

public class ExternalProofCreateDto : ProofCreateDto
{
    public string EventName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string OrganizationName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public double Score { get; set; }
    
    public new DateTime? AttendanceAt { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    
    public Guid ActivityId { get; set; }
}