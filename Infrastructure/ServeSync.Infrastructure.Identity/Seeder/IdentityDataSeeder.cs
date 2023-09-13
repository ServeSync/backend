using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;

namespace ServeSync.Infrastructure.Identity.Seeder;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<IdentityDataSeeder> _logger;

    public IdentityDataSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<IdentityDataSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (
                !_userManager.Users.Any()
            )
            {
                _logger.LogInformation("Begin seeding identity data...");
                
                await SeedDefaultAdminAccountAsync();

                _logger.LogInformation("Seed identity data successfully!");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Seeding identity data failed!", ex);
        }
    }

    private async Task SeedDefaultAdminAccountAsync()
    {
        var user = new ApplicationUser()
        {
            UserName = "admin",
            Email = "admin@gmail.com",
        };

        await _userManager.CreateAsync(user, "admin123");
        await _userManager.AddToRoleAsync(user, AppRole.Admin);
    }
}