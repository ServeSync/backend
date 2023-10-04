using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;

public class NewStudentCreatedDomainEvent : IDomainEvent
{
    public Student Student { get; set; }
    
    public NewStudentCreatedDomainEvent(Student student)
    {
        Student = student;
    }
}