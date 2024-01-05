using System.Security.Claims;
using ServeSync.Application.Common;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Entities;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.Specifications;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Domain.StudentManagement.StudentAggregate.Entities;
using ServeSync.Domain.StudentManagement.StudentAggregate.Specifications;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Services;

public class IdentityUserClaimGenerator
{
    private readonly IBasicReadOnlyRepository<EventOrganization, Guid> _eventOrganizationRepository;
    private readonly IBasicReadOnlyRepository<EventOrganizationContact, Guid> _eventOrganizationContactRepository;
    private readonly IBasicReadOnlyRepository<Student, Guid> _studentRepository;
    
    public IdentityUserClaimGenerator(
        IBasicReadOnlyRepository<EventOrganization, Guid> eventOrganizationRepository,
        IBasicReadOnlyRepository<EventOrganizationContact, Guid> eventOrganizationContactRepository,
        IBasicReadOnlyRepository<Student, Guid> studentRepository)
    {
        _eventOrganizationRepository = eventOrganizationRepository;
        _eventOrganizationContactRepository = eventOrganizationContactRepository;
        _studentRepository = studentRepository;
    }
    
    public async Task<IEnumerable<Claim>> GenerateAsync(ApplicationUser user, Guid? tenantId = null)
    {
        var claims = new List<Claim>()
        {
            new (AppClaim.UserId, user.Id),
            new (AppClaim.UserName, user.UserName),
            new (AppClaim.Email, user.Email)
        };

        if (tenantId != null)
        {
            if (user.Tenants.All(x => x.TenantId != tenantId.Value))
            {
                throw new UserNotInTenantException(user.Id, tenantId.Value);
            }
            
            claims.Add(new Claim(AppClaim.TenantId, tenantId.Value.ToString()));
        }
        else
        {
            var tenant = user.Tenants.FirstOrDefault();
            if (tenant != null)
            {
                tenantId = tenant.TenantId;
                claims.Add(new Claim(AppClaim.TenantId, tenant.TenantId.ToString()));
            }    
        }
        
        claims.Add(new (AppClaim.ReferenceId, await GetReferenceIdAsync(user, tenantId!.Value)));

        return claims;
    }
    
    private async Task<string> GetReferenceIdAsync(ApplicationUser user, Guid tenantId)
    {
        switch (user.GetDefaultRole(tenantId))
        {
            case AppRole.EventOrganization:
                var eventOrganization = await _eventOrganizationRepository.FindAsync(new EventOrganizationByIdentitySpecification(user.Id));
                if (eventOrganization == null)
                    break;

                return eventOrganization.Id.ToString();
            
            case AppRole.EventOrganizer:
                var eventOrganizer = await _eventOrganizationContactRepository.FindAsync(new EventOrganizationContactByIdentitySpecification(user.Id));
                if (eventOrganizer == null)
                    break;
                
                return eventOrganizer.Id.ToString();
            
            case AppRole.Student:
                var student = await _studentRepository.FindAsync(new StudentByIdentitySpecification(user.Id));
                if (student == null)
                    break;

                return student.Id.ToString();
        }

        return user.ExternalId.ToString();
    }
}