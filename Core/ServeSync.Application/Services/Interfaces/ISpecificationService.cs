using ServeSync.Domain.EventManagement.EventAggregate.Entities;
using ServeSync.Domain.SeedWorks.Specifications.Interfaces;

namespace ServeSync.Application.Services.Interfaces;

public interface ISpecificationService
{ 
    Task<ISpecification<Event, Guid>> GetEventPersonalizedSpecificationAsync();
}