﻿using ServeSync.Domain.SeedWorks.Models;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

public class Student : AggregateRoot
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

    public string IdentityId { get; private set; }

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
        string identityId,
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
        IdentityId = Guard.NotNullOrEmpty(identityId, nameof(IdentityId));
        HomeTown = homeTown;
        Address = address;
    }

    private Student()
    {
        
    }
}