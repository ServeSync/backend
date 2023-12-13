using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.DomainEvents;

public class ProofApprovedDomainEvent : IDomainEvent
{
    public Proof Proof { get; set; }
    
    public ProofApprovedDomainEvent(Proof proof)
    {
        Proof = proof;
    }
}