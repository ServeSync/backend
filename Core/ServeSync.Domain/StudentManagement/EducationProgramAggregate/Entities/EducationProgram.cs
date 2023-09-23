using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.StudentManagement.EducationProgramAggregate.Entities;

public class EducationProgram : AggregateRoot
{
    public string Name { get; private set; }
    public int RequiredActivityScore { get; private set; }
    public int RequiredCredit { get; private set; }

    public EducationProgram(string name, int requiredActivityScore, int requiredCredit)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(Name));
        RequiredActivityScore = Guard.Positive(requiredActivityScore, nameof(RequiredActivityScore));
        RequiredCredit = Guard.Positive(requiredCredit, nameof(RequiredCredit));
    }

    private EducationProgram()
    {
        
    }
}