using System.Reflection;
using System.Text;
using CloudinaryDotNet;
using FluentValidation;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MySqlConnector;
using ServeSync.API.Authorization;
using ServeSync.API.Common.ExceptionHandlers;
using ServeSync.Application;
using ServeSync.Application.Common.Dtos;
using ServeSync.Application.Common.Settings;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Application.Services;
using ServeSync.Application.Services.Interfaces;
using ServeSync.Domain.SeedWorks.Repositories;
using ServeSync.Infrastructure.EfCore;
using ServeSync.Infrastructure.EfCore.Repositories;
using ServeSync.Infrastructure.EfCore.Repositories.Base;
using ServeSync.Infrastructure.EfCore.UnitOfWorks;
using ServeSync.Infrastructure.Gmail;
using ServeSync.Infrastructure.Identity;
using ServeSync.Infrastructure.Identity.Models.PermissionAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate;
using ServeSync.Infrastructure.Identity.Models.RoleAggregate.Entities;
using ServeSync.Infrastructure.Identity.Models.UserAggregate;
using ServeSync.Infrastructure.Identity.Models.UserAggregate.Entities;
using ServeSync.Infrastructure.Identity.Seeder;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Dtos;
using ServeSync.Infrastructure.Identity.UseCases.Auth.Settings;
using ServeSync.Application.Caching;
using ServeSync.Application.Caching.Interfaces;
using ServeSync.Application.DomainEventHandlers.EventManagement.Events;
using ServeSync.Application.Identity;
using ServeSync.Application.ImageUploader;
using ServeSync.Application.MailSender;
using ServeSync.Application.MailSender.Interfaces;
using ServeSync.Application.QueryObjects;
using ServeSync.Application.ReadModels.Abstracts;
using ServeSync.Application.ReadModels.Events;
using ServeSync.Application.Seeders;
using ServeSync.Application.SeedWorks.Behavior;
using ServeSync.Application.SeedWorks.Schedulers;
using ServeSync.Domain.EventManagement.EventAggregate;
using ServeSync.Domain.EventManagement.EventAggregate.DomainEvents;
using ServeSync.Domain.EventManagement.EventAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventCategoryAggregate;
using ServeSync.Domain.EventManagement.EventCategoryAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate;
using ServeSync.Domain.EventManagement.EventCollaborationRequestAggregate.DomainServices;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate;
using ServeSync.Domain.EventManagement.EventOrganizationAggregate.DomainServices;
using ServeSync.Domain.SeedWorks.Events;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate;
using ServeSync.Domain.StudentManagement.EducationProgramAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.FacultyAggregate;
using ServeSync.Domain.StudentManagement.FacultyAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate;
using ServeSync.Domain.StudentManagement.HomeRoomAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.StudentAggregate;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainEvents;
using ServeSync.Domain.StudentManagement.StudentAggregate.DomainServices;
using ServeSync.Infrastructure.Caching;
using ServeSync.Infrastructure.Cloudinary;
using ServeSync.Infrastructure.Dappers;
using ServeSync.Infrastructure.Dappers.QueryObjects;
using ServeSync.Infrastructure.HangFire;
using ServeSync.Infrastructure.Identity.Caching;
using ServeSync.Infrastructure.Identity.Caching.Interfaces;
using ServeSync.Infrastructure.Identity.Commons.Constants;
using ServeSync.Infrastructure.Identity.Services;
using ServeSync.Infrastructure.MongoDb;
using ServeSync.Infrastructure.MongoDb.Repositories;
using ServeSync.Infrastructure.MongoDb.Settings;

namespace ServeSync.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ForgetPasswordSetting>(configuration.GetSection("ForgetPasswordSetting"));
        
        services.AddScoped<IExceptionHandler, ExceptionHandler>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        
        return services;
    }
    
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ServeSync",
                Version = "v1"
            });

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                BearerFormat = "JWT",
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
    
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICachingService, ServeSyncDistributedCachingService>();
        services.AddTransient<IEducationCachingManager, EducationCachingManager>();
        services.AddTransient<IHomeRoomCachingManager, HomeRoomCachingManager>();
        services.AddTransient<IFacultyCachingManager, FacultyCachingManager>();
        services.AddTransient<IUserCacheManager, UserCacheManager>();
        services.AddTransient<IPermissionCacheManager, PermissionCacheManager>();
        services.AddTransient<IEventCachingManager, EventCachingManager>();
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ServeSync";
        });

        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.EnableSensitiveDataLogging(env.IsDevelopment());
            options.UseMySQL(configuration.GetConnectionString("Default"));
        });

        services.AddTransient<ISqlQuery, DapperSqlQuery>();
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        
        services.AddScoped(typeof(IRepository<,>), typeof(EfCoreRepository<,>));
        services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(EfCoreReadOnlyRepository<,>));
        services.AddScoped(typeof(IBasicReadOnlyRepository<,>), typeof(EfCoreBasicReadOnlyRepository<,>));
        services.AddScoped(typeof(ISpecificationRepository<,>), typeof(EfCoreSpecificationRepository<,>));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IEducationProgramRepository, EducationProgramRepository>();
        services.AddScoped<IHomeRoomRepository, HomeRoomRepository>();
        services.AddScoped<IFacultyRepository, FacultyRepository>();
        services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
        services.AddScoped<IEventCollaborationRequestRepository, EventCollaborationRequestRepository>();
        services.AddScoped<IEventOrganizationRepository, EventOrganizationRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSetting>(configuration.GetSection("Jwt"));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
        
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Headers.TryGetValue("X-AccessToken", out StringValues headerValue))
                        {
                            var bearerPrefix = "Bearer ";
                            var token = headerValue.ToString();
                            if (!string.IsNullOrEmpty(token) && token.StartsWith(bearerPrefix))
                            {
                                token = token[bearerPrefix.Length..];
                            }

                            context.Token = token;
                        }

                        return Task.CompletedTask;    
                    }
                };
            });

        return services;
    }

    public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, EventAccessControlAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, ServeSyncAuthorizationPolicyProvider>();

        return services;
    }
    
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServeSyncApplicationReference));
        services.AddAutoMapper(typeof(ServeSyncIdentityReference));
        
        return services;
    }
    
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;

            // Signin settings.
            options.SignIn.RequireConfirmedEmail = false;
        });

        return services;
    }
    
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(ServeSyncApplicationReference)));
            config.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(ServeSyncIdentityReference)));
            
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(ServeSyncApplicationReference)));
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(ServeSyncIdentityReference)));
        
        return services;
    }

    public static IServiceCollection AddApplicationCors(this IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy("ServeSync", builder =>
        {
            builder.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        return services;
    }

    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSetting>(configuration.GetSection("Email"));
        services.AddScoped<IEmailTemplateGenerator, EmailTemplateGenerator>();
        services.AddScoped<IEmailSender, GmailSender>();
        
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromMinutes(int.Parse(configuration["ForgetPasswordSetting:ExpiresInMinute"]))
        );

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentDomainService, StudentDomainService>();
        services.AddScoped<IFacultyDomainService, FacultyDomainService>();
        services.AddScoped<IEducationProgramDomainService, EducationProgramDomainService>();
        services.AddScoped<IHomeRoomDomainService, HomeRoomDomainService>();
        services.AddScoped<IEventCategoryDomainService, EventCategoryDomainService>();
        services.AddScoped<IEventOrganizationDomainService, EventOrganizationDomainService>();
        services.AddScoped<IEventCollaborationRequestDomainService, EventCollaborationRequestDomainService>();
        services.AddScoped<IEventDomainService, EventDomainService>();

        return services;
    }

    public static IServiceCollection AddPersistedDomainEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<IPersistedDomainEventHandler<EventUpdatedDomainEvent>, SyncEventReadModelDomainEventHandler>();
        services.AddScoped<IPersistedDomainEventHandler<NewEventCreatedDomainEvent>, SyncEventReadModelDomainEventHandler>();
        services.AddScoped<IPersistedDomainEventHandler<StudentEventRegisterApprovedDomainEvent>, SyncEventReadModelDomainEventHandler>();
        services.AddScoped<IPersistedDomainEventHandler<StudentEventRegisterRejectedDomainEvent>, SyncEventReadModelDomainEventHandler>();
        services.AddScoped<IPersistedDomainEventHandler<StudentAttendedToEventDomainEvent>, SyncEventReadModelDomainEventHandler>();
        services.AddScoped<IPersistedDomainEventHandler<StudentRegisteredToEventRoleDomainEvent>, SyncEventReadModelDomainEventHandler>();

        return services;
    }

    public static IServiceCollection AddDataSeeders(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, IdentityDataSeeder>();
        services.AddScoped<IDataSeeder, PermissionDataSeeder>();
        services.AddScoped<IDataSeeder, StudentManagementDataSeeder>();
        services.AddScoped<IDataSeeder, EventManagementDataSeeder>();
        
        return services;
    }
    
    public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CloudinaryDotNet.Cloudinary>(provider =>
        {
            var cloudinarySection = configuration.GetSection("Cloudinary");
            var account = new Account()
            {
                Cloud = cloudinarySection["CloudName"],
                ApiKey = cloudinarySection["ApiKey"],
                ApiSecret = cloudinarySection["ApiSecret"]
            };
            
            return new CloudinaryDotNet.Cloudinary(account);
        });

        services.AddScoped<IImageUploader, CloudinaryImageUploader>();
        
        return services;
    }

    public static IServiceCollection AddHangFireBackGroundJob(this IServiceCollection services, IConfiguration configuration)
    {
        InitHangFireDb(configuration);
        
        services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(
                    new MySqlStorage(configuration.GetConnectionString("HangFire"), new MySqlStorageOptions()
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(3),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        PrepareSchemaIfNecessary = true
                    })
                ));

        services.AddScoped<IBackGroundJobManager, HangFireBackGroundJobManager>();
        services.AddScoped<IBackGroundJobPublisher, BackGroundJobPublisher>();
        services.AddHangfireServer();
        
        return services;
    }
    
    public static IServiceCollection AddQueryObjects(this IServiceCollection services)
    {
        services.AddTransient<IEventQueries, EventQueries>();
        
        return services;
    }

    public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Utc));
 
        services.AddSingleton(provider =>
        {
            var mongoDbSettings = configuration.GetSection("MongoDb").Get<MongoDbSetting>() ?? throw new Exception("MongoDb setting is not provided!");
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase("ServeSync");
        });

        services.AddMongoRepository<EventReadModel, Guid>("Events");
        services.AddSingleton<IEventReadModelRepository>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            return new EventReadModelRepository(database, "Events");
        });
        
        return services;
    }

    private static void InitHangFireDb(IConfiguration configuration)
    {
        using var connection = new MySqlConnection(configuration.GetConnectionString("HangFireMaster"));
        connection.Open();
            
        var dbName = new MySqlConnection(configuration.GetConnectionString("HangFire")).Database;
        
        using var command = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {dbName};", connection);
        command.ExecuteNonQuery();
    }
    
    private static IServiceCollection AddMongoRepository<T, TKey>(this IServiceCollection services, string collectionName) where T : BaseReadModel<TKey> where TKey : IEquatable<TKey>
    {
        services.AddSingleton<IReadModelRepository<T, TKey>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            return new MongoDbRepository<T, TKey>(database, collectionName);
        });
        return services;
    }
}