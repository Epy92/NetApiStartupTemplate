using Infrastructure.DatabaseModels;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
	public static class InfrastructureRegistrationExtensions
	{
		public static IServiceCollection AddDatabaseInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
			services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
            services.AddHealthChecks().AddDbContextCheck<DatabaseContext>();

            //Register the current project's dependencies
            services.Scan(scanner => scanner.FromAssemblies(typeof(InfrastructureRegistrationExtensions).Assembly)
                .AddClasses(c => c.Where(type => !type.IsNested), publicOnly: false)
                .AsSelfWithInterfaces().WithTransientLifetime());

            using (var db = new DatabaseContext())
            {
                try
                {
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    //ToDo: LOG error
                }
            }

            return services;
		}
	}
}
