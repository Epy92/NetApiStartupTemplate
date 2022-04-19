using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class InfrastructureRegistrationExtensions
	{
		public static IServiceCollection AddDatabaseInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
			services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

			//Register the current project's dependencies
			services.Scan(scanner => scanner.FromAssemblies(typeof(InfrastructureRegistrationExtensions).Assembly)
                .AddClasses(c => c.Where(type => !type.IsNested), publicOnly: false)
                .AsSelfWithInterfaces().WithTransientLifetime());


            return services;
		}
	}
}
