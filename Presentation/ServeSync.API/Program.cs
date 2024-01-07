using Newtonsoft.Json.Converters;
using Serilog;
using ServeSync.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = DependencyInjectionExtensions.CreateSerilogLogger(builder.Configuration);

builder.Services
    .AddHttpContextAccessor()
    .AddServices(builder.Configuration)
    .AddApplicationCors()
    .AddDatabase(builder.Configuration, builder.Environment)
    .AddIdentity(builder.Configuration, builder.Environment)
    .AddRepositories()
    .AddApplicationAuthentication(builder.Configuration)
    .AddApplicationAuthorization()
    .AddSwagger()
    .AddMapper()
    .AddCqrs()
    .AddRedisCache(builder.Configuration)
    .AddEmailSender(builder.Configuration)
    .AddDomainServices()
    .AddDataSeeders(builder.Environment)
    .AddCloudinary(builder.Configuration)
    .AddQueryObjects()
    .AddMongoDB(builder.Configuration)
    .AddPersistedDomainEventHandlers()
    .AddHangFireBackGroundJob(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Host.UseSerilog();

var app = builder.Build();

app.UseExceptionHandling();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ServeSync");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.ApplyMigrationAsync(app.Logger);
await app.SeedDataAsync();

app.Run();