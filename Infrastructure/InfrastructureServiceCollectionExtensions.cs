using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class InfrastructureServiceCollectionExtensions
	{
		public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
		{
			services.AddSingleton<IEmailSender, SmtpEmailSender>();
			return services;
		}

		public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
		{
			// TODO:

			return services;
		}
	}
}
