﻿using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.StudentAggregate.Enums;
using ServeSync.Domain.StudentManagement.StudentAggregate.Exceptions;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

public class Student : AuditableAggregateRoot
{
    public string Code { get; private set; }
    public string FullName { get; private set; }
    public bool Gender { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? HomeTown { get; private set; }
    public string? Address { get; private set; }
    public string ImageUrl { get; private set; }
    public string CitizenId { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    
    public Guid HomeRoomId { get; private set; }
    public HomeRoom? HomeRoom { get; private set; }
    
    public Guid EducationProgramId { get; private set; }
    public EducationProgram? EducationProgram { get; private set; }

    public string IdentityId { get; private set; } = null!;
    
    public List<StudentEventRegister> EventRegisters { get; private set; }
    public List<Proof> Proofs { get; private set; }

    internal Student(
        string code,
        string fullName,
        bool gender,
        DateTime dateOfBirth,
        string imageUrl,
        string citizenId,
        string email,
        string phone,
        Guid homeRoomId,
        Guid educationProgramId,
        string? homeTown = null,
        string? address = null)
    {
        Code = Guard.NotNullOrEmpty(code, nameof(Code));
        FullName = Guard.NotNullOrEmpty(fullName, nameof(FullName));
        Gender = gender;
        DateOfBirth = dateOfBirth;
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        CitizenId = Guard.NotNullOrEmpty(citizenId, nameof(CitizenId));
        Email = Guard.NotNullOrEmpty(email, nameof(Email));
        Phone = Guard.NotNullOrEmpty(phone, nameof(Phone));
        HomeRoomId = Guard.NotNull(homeRoomId, nameof(HomeRoomId));
        EducationProgramId = Guard.NotNull(educationProgramId, nameof(EducationProgramId));
        HomeTown = homeTown;
        Address = address;

        EventRegisters = new List<StudentEventRegister>();
        
        AddDomainEvent(new NewStudentCreatedDomainEvent(this));
    }

    internal void WithIdentity(string identityId)
    {
        IdentityId = Guard.NotNullOrEmpty(identityId, nameof(IdentityId));;
    }

    internal void UpdateContactInfo(
        string fullName, 
        bool gender, 
        DateTime dateOfBirth, 
        string imageUrl,
        string citizenId,
        string email, 
        string phone, 
        string? homeTown = null, 
        string? address = null)
    {
        if (IsFullNameChanged(fullName) || IsEmailChanged(email) || IsImageUrlChanged(imageUrl))
        {
            AddDomainEvent(new StudentContactInfoUpdatedDomainEvent(Id, fullName, email, imageUrl, IdentityId));    
        }
        
        FullName = Guard.NotNullOrEmpty(fullName, nameof(FullName));
        Gender = gender;
        DateOfBirth = dateOfBirth;
        ImageUrl = Guard.NotNullOrEmpty(imageUrl, nameof(ImageUrl));
        CitizenId = Guard.NotNullOrEmpty(citizenId, nameof(CitizenId));
        Email = Guard.NotNullOrEmpty(email, nameof(Email));
        Phone = Guard.NotNullOrEmpty(phone, nameof(Phone));
        HomeTown = homeTown;
        Address = address;
        
        AddDomainEvent(new StudentUpdatedDomainEvent(Id));
    }
    
    internal void UpdateEducationInfo(string code, Guid educationProgramId, Guid homeRoomId)
    {
        if (IsCodeChanged(code))
        {
            AddDomainEvent(new StudentCodeUpdatedDomainEvent(Id, code, IdentityId));    
        }
        
        Code = Guard.NotNullOrEmpty(code, nameof(Code));
        EducationProgramId = Guard.NotNull(educationProgramId, nameof(EducationProgramId));
        HomeRoomId = Guard.NotNull(homeRoomId, nameof(HomeRoomId));
        
        AddDomainEvent(new StudentUpdatedDomainEvent(Id));
    }
    
    internal void RegisterEvent(Guid eventRoleId, string? description = null)
    {
        if (EventRegisters.Any(x => x.EventRoleId == eventRoleId && x.Status == EventRegisterStatus.Pending))
        {
            throw new StudentRegisteredToEventRoleException(Id, eventRoleId);
        }
        
        var eventRegister = new StudentEventRegister(description, eventRoleId, Id);
        EventRegisters.Add(eventRegister);
        
        AddDomainEvent(new StudentRegisteredToEventRoleDomainEvent(this, eventRoleId));
    }
    
    internal void RegisterEventWithApprove(Guid eventRoleId, string? description = null)
    {
        if (EventRegisters.Any(x => x.EventRoleId == eventRoleId && x.Status == EventRegisterStatus.Pending))
        {
            throw new StudentRegisteredToEventRoleException(Id, eventRoleId);
        }
        
        var eventRegister = new StudentEventRegister(description, eventRoleId, Id);
        eventRegister.Approve();
        
        EventRegisters.Add(eventRegister);
    }
    
    internal void ApproveEventRegister(Guid eventRegisterId)
    {
        var eventRegister = EventRegisters.FirstOrDefault(x => x.Id == eventRegisterId);
        if (eventRegister == null)
        {
            throw new StudentEventRegisterNotFoundException(eventRegisterId);
        }
        
        eventRegister.Approve();
    }

    internal void RejectEventRegister(Guid eventRegisterId, string reason)
    {
        var eventRegister = EventRegisters.FirstOrDefault(x => x.Id == eventRegisterId);
        if (eventRegister == null)
        {
            throw new StudentEventRegisterNotFoundException(eventRegisterId);
        }
        
        eventRegister.Reject(reason);
    }
    
    internal void AttendEvent(Guid eventRoleId, Guid eventAttendanceInfoId)
    {
        var eventRegister = EventRegisters.FirstOrDefault(x => x.EventRoleId == eventRoleId);
        if (eventRegister == null)
        {
            throw new StudentNotApprovedToEventException(Id, eventRoleId);
        }
        
        eventRegister.Attendance(eventAttendanceInfoId);
    }
    
    internal bool IsApprovedToEventRole(Guid eventRoleId)
    {
        return EventRegisters.Any(x => x.EventRoleId == eventRoleId && x.Status == EventRegisterStatus.Approved);
    }

    private bool IsCodeChanged(string code)
    {
        return Code != code;
    }
    
    private bool IsFullNameChanged(string fullName)
    {
        return FullName != fullName;
    }
    
    private bool IsEmailChanged(string email)
    {
        return Email != email;
    }

    private bool IsImageUrlChanged(string imageUrl)
    {
        return ImageUrl != imageUrl;
    }

    private Student()
    {
        
    }
}