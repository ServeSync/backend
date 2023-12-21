using Microsoft.EntityFrameworkCore;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Infrastructure.EfCore.Common;
using ServeSync.Infrastructure.EfCore.Repositories.Base;

namespace ServeSync.Infrastructure.EfCore.Repositories;

public class StudentRepository : EfCoreRepository<Student>, IStudentRepository
{
    private readonly ISpecificationService _specificationService;
    public StudentRepository(ISpecificationService specificationService, AppDbContext dbContext) : base(dbContext)
    {
        _specificationService = specificationService;
        
        AddInclude(x => x.EventRegisters);
        AddInclude("EventRegisters.StudentEventAttendance");
    }

    public Task<Student?> FindByIdentityAsync(string identityId)
    {
        return GetQueryable().FirstOrDefaultAsync(x => x.IdentityId == identityId);
    }

    public async Task<IQueryable<StudentEventRegister>> GetEventStudentRegisteredQueryable(ISpecification<StudentEventRegister, Guid> specification)
    {
        var personalizedSpecification = await _specificationService.GetEventPersonalizedSpecificationAsync();
        var eventIdQueryable = SpecificationEvaluator<Event, Guid>
            .GetQuery(DbContext.Set<Event>(), personalizedSpecification)
            .Select(x => x.Id);

        var queryable = SpecificationEvaluator<StudentEventRegister, Guid>
            .GetQuery(DbContext.Set<StudentEventRegister>(), specification)
            .Where(x => eventIdQueryable.Contains(x.EventRole!.EventId));

        return queryable;
    }

    public async Task<IQueryable<StudentEventAttendance>> GetEventStudentAttendanceQueryable(ISpecification<StudentEventAttendance, Guid> specification)
    {
        var personalizedSpecification = await _specificationService.GetEventPersonalizedSpecificationAsync();
        var eventIdQueryable = SpecificationEvaluator<Event, Guid>
            .GetQuery(DbContext.Set<Event>(), personalizedSpecification)
            .Select(x => x.Id);
        
        var queryable = SpecificationEvaluator<StudentEventAttendance, Guid>
            .GetQuery(DbContext.Set<StudentEventAttendance>(), specification)
            .Where(x => eventIdQueryable.Contains(x.StudentEventRegister!.EventRole!.EventId));
        
        return queryable;
    }
}