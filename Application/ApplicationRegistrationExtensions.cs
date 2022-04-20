using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Application
{
    public static class ApplicationRegistrationExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Use the invariant culture throughout the application
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            services.AddDatabaseInfrastructureLayer(configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            // Register the current project's dependencies
            services.Scan(scanner => scanner.FromAssemblies(typeof(ApplicationRegistrationExtensions).Assembly)
                .AddClasses(c => c.Where(type => !type.IsNested), publicOnly: false)
                .AsSelfWithInterfaces().WithTransientLifetime());

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var key = Encoding.ASCII.GetBytes(appSettingsSection.GetSection("JwtSecret").Value);

            services.AddAutoMapper(typeof(ApplicationRegistrationExtensions));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(key),
                   };
                   options.Events = new JwtBearerEvents
                   {
                   };
               });

            ////Authorization
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("defaultpolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme };
                });
            });

            return services;
        }

        public static void SeedDatabase(IServiceProvider serviceProvider, IConfiguration configuration) 
        {
            Seed.CreateRoles(serviceProvider, configuration).Wait();
        }
    }
}
