﻿using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.StudentManagement.FacultyAggregate.Entities;

public class Faculty : AggregateRoot
{
    public string Name { get; set; }

    internal Faculty(string name)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
    }
    
    private Faculty()
    {
        
    }
}