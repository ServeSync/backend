using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServeSync.Application.Common;
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
            _logger.LogInformation("Begin seeding identity data...");
            
            // Admin role seed by migration
            if (!await _roleManager.Roles.AnyAsync())
            {
                await SeedStudentRoleAsync();   
                await SeedStudentAffairRoleAsync();
                await SeedEventOrganizerRoleAsync();
                await SeedEventOrganizationRoleAsync();
            }
            
            if (!await _userManager.Users.AnyAsync())
            {
                await SeedDefaultAdminAccountAsync();
                await SeedDefaultStudentAccountAsync();
                await SeedDefaultStudentAffairAccountAsync();
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
        var users = new List<ApplicationUser>()
        {
            new ApplicationUser("Nguyễn Hùng Ngọc")
            {
                UserName = "admin1",
                Email = "ngocnguyen752002@gmail.com"
            },
            new ApplicationUser("Lê Quốc Rôn")
            {
                UserName = "admin2",
                Email = "ronle9519@gmail.com"
            },
            new ApplicationUser("Huỳnh Tấn Năng")
            {
                UserName = "admin3",
                Email = "tannang09032002@gmail.com"
            },
            new ApplicationUser("Trương Tuấn Kiệt")
            {
                UserName = "admin4",
                Email = "tuankiettruong.work@gmail.com"
            },
            new ApplicationUser("Trần Thị Kim Quy")
            {
                UserName = "admin5",
                Email = "trankimquy1952@gmail.com"
            },
            new ApplicationUser("Lê Thị Admin")
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            }
        };

        foreach (var user in users)
        {
            await _userManager.CreateAsync(user, "admin123");
            await _userManager.AddToRoleAsync(user, AppRole.Admin);
        };
    }
    
    private async Task SeedDefaultStudentAccountAsync()
    {
        var users = new List<ApplicationUser>()
        {
            new ApplicationUser("Nguyễn Thị Student")
            {
                UserName = "student",
                Email = "student@gmail.com"
            }
        };

        foreach (var user in users)
        {
            await _userManager.CreateAsync(user, "student123");
            await _userManager.AddToRoleAsync(user, AppRole.Student);
        };
    }

    private async Task SeedDefaultStudentAffairAccountAsync()
    {
        var users = new List<ApplicationUser>()
        {
            new ApplicationUser("Nguyễn Thị Student Affair")
            {
                UserName = "student",
                Email = "minishop.pbl3@gmail.com"
            }
        };

        foreach (var user in users)
        {
            await _userManager.CreateAsync(user, "student123");
            await _userManager.AddToRoleAsync(user, AppRole.StudentAffair);
        }; 
    }

    private Task SeedStudentRoleAsync()
    {
        var studentRole = new ApplicationRole(AppRole.Student);
        return _roleManager.CreateAsync(studentRole);
    }
    
    private Task SeedStudentAffairRoleAsync()
    {
        var studentAffairRole = new ApplicationRole(AppRole.StudentAffair);
        return _roleManager.CreateAsync(studentAffairRole);
    }
    
    private Task SeedEventOrganizerRoleAsync()
    {
        var eventOrganizerRole = new ApplicationRole(AppRole.EventOrganizer);
        return _roleManager.CreateAsync(eventOrganizerRole);
    }
    
    private Task SeedEventOrganizationRoleAsync()
    {
        var eventOrganizerRole = new ApplicationRole(AppRole.EventOrganization);
        return _roleManager.CreateAsync(eventOrganizerRole);
    }
}