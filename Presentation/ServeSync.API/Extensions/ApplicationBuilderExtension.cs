using Microsoft.AspNetCore.Diagnostics;
using ServeSync.API.Common.ExceptionHandlers;

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
}