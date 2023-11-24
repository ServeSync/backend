using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Common;
using ServeSync.Application.Identity;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.TenantAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Seeder;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly IIdentityService _identityService;
    private readonly ITenantService _tenantService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<IdentityDataSeeder> _logger;

    public IdentityDataSeeder(
        IIdentityService identityService,
        ITenantService tenantService,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<IdentityDataSeeder> logger)
    {
        _identityService = identityService;
        _tenantService = tenantService;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Begin seeding identity data...");
            
            if (!await _userManager.Users.AnyAsync())
            {
                await SeedDefaultAdminAccountAsync();
            }
            
            _logger.LogInformation("Seed identity data successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Seeding identity data failed: {Message}!", ex.Message);
        }
    }

    private async Task SeedDefaultAdminAccountAsync()
    {
        var user = await _identityService.CreateUserAsync(
            "ServeSync",
            "admin",
            "https://res.cloudinary.com/dboijruhe/image/upload/v1700832514/Assets/ddxrzqclm5ysut4o9qie.png",
            "admin@gmail.com",
            "admin123");

        if (user.IsSuccess)
        {
            await _tenantService.AddUserToTenantAsync(
                user.Data!.Id, 
                "ServeSync",
                "https://res.cloudinary.com/dboijruhe/image/upload/v1700832514/Assets/ddxrzqclm5ysut4o9qie.png", true,
                AppTenant.Default);
            await _identityService.GrantToRoleAsync(user.Data!.Id, AppRole.Admin, AppTenant.Default);    
        }
    }
}