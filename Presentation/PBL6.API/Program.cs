using PBL6.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices()
                .AddApplicationCors()
                .AddDatabase(builder.Configuration, builder.Environment)
                .AddIdentity(builder.Configuration, builder.Environment)
                .AddRepositories()
                .AddApplicationAuthentication(builder.Configuration)
                .AddApplicationAuthorization()
                .AddSwagger()
                .AddMapper()
                .AddRedisCache(builder.Configuration);

builder.Services.AddControllers();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

app.UseExceptionHandling();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("pbl6");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();