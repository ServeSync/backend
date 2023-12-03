using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.ProofAggregate.Entities;

namespace ServeSync.Domain.StudentManagement.ProofAggregate.DomainEvents;

public class ProofRejectedDomainEvent : IDomainEvent
{
    public Proof Proof { get; set;  }
    public string RejectReason { get; set; }
    public ProofRejectedDomainEvent(Proof proof, string rejectReason)
    {
        Proof = proof;
        RejectReason = rejectReason;
    }
}
