using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ServeSync.API.Common.ExceptionHandlers;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Infrastructure.EfCore;

namespace ServeSync.API.Extensions;

public static class ApplicationBuilderExtension
{
    public static void UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                var exception = exceptionHandlerPathFeature?.Error;

                if (exception != null)
                {
                    using var scope = app.ApplicationServices.CreateScope();

                    var exceptionHandler = scope.ServiceProvider.GetRequiredService<IExceptionHandler>();
                    await exceptionHandler.HandleAsync(context, exception);
                }
            });
        });
    }
    
    public static async Task ApplyMigrationAsync(this IApplicationBuilder app, ILogger logger)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            logger.LogInformation("Migrating pending migration...");   

            await context.Database.MigrateAsync();

            logger.LogInformation("Migrated successfully!");
        }
        else
        {
            logger.LogInformation("No pending migration!");
        }
    }

    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDataSeeder>>();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
    }
}