﻿using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;

public interface IProofDomainService
{
    Task<Proof> CreateInternalProofAsync(
        string? description, 
        string imageUrl, 
        DateTime attendanceAt, 
        Guid studentId,
        Guid eventId,
        Guid eventRoleId,
        DateTime dateTime);

    Task<Proof> CreateExternalProofAsync(
        string? description,
        string imageUrl,
        DateTime? attendanceAt,
        Guid studentId,
        string eventName,
        string organizationName,
        string address,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId);
    
    Task<Proof> CreateSpecialProofAsync(
        string? description,
        string imageUrl,
        Guid studentId,
        string title,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId);
    
    Task<Proof> UpdateInternalProofAsync(
        Proof proof,
        string? description, 
        string imageUrl, 
        DateTime attendanceAt, 
        Guid eventId,
        Guid eventRoleId,
        DateTime dateTime);

    Task<Proof> UpdateExternalProofAsync(
        Proof proof,
        string? description,
        string imageUrl,
        DateTime? attendanceAt,
        string eventName,
        string organizationName,
        string address,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId);
    
    Task<Proof> UpdateSpecialProofAsync(
        Proof proof,
        string? description,
        string imageUrl,
        string title,
        string role,
        DateTime startAt,
        DateTime endAt,
        double score,
        Guid activityId);

    Proof RejectProof(Proof proof, string reason);
    
    Task<Proof> ApproveProofAsync(Proof proof);
    
    void Delete(Proof proof);
}