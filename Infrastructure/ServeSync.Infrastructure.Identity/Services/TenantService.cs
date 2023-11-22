using AutoMapper;
using ServeSync.Application.Identity;
using ServeSync.Application.Identity.Dtos;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate.Exceptions;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Exceptions;

namespace ServeSync.Infrastructure.Identity.Services;

public class TenantService : ITenantService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public TenantService(
        IMapper mapper,
        IUserRepository userRepository,
        ITenantRepository tenantRepository, 
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IdentityResult<TenantDto>> CreateAsync(string name, string avatarUrl)
    {
        var tenant = new Tenant(name, avatarUrl);
        
        await _tenantRepository.InsertAsync(tenant);
        await _unitOfWork.CommitAsync();

        return IdentityResult<TenantDto>.Success(_mapper.Map<TenantDto>(tenant));
    }

    public async Task UpdateAsync(Guid tenantId, string name, string avatarUrl)
    {
        var tenant = await _tenantRepository.FindByIdAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException(tenantId);
        }

        tenant.Update(name, avatarUrl);
        _tenantRepository.Update(tenant);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid tenantId)
    {
        var tenant = await _tenantRepository.FindByIdAsync(tenantId);
        if (tenant == null)
        {
            throw new TenantNotFoundException(tenantId);
        }
        
        _tenantRepository.Delete(tenant);
        await _unitOfWork.CommitAsync();
    }

    public async Task AddUserToTenantAsync(string userId, string fullname, string avatarUrl, bool isOwner, Guid tenantId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        if (!await _tenantRepository.IsExistingAsync(tenantId))
        {
            throw new TenantNotFoundException(tenantId);
        }
        
        user.AddToTenant(fullname, avatarUrl, tenantId, isOwner);
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateUserInTenantAsync(string userId, Guid tenantId, string fullname, string avatarUrl)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }
        
        user.UpdateProfileInTenant(fullname, avatarUrl, tenantId);
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoveUserFromTenantAsync(string userId, Guid tenantId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }
        
        user.RemoveFromTenant(tenantId);
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }
}