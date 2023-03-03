using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
	public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
		ConfigurationManager configuration)
	{
		services.AddSingleton<IEmailSender, SmtpEmailSender>();
		var configurationSection = configuration.GetSection(nameof(RegionalTaxInfoFactory.RegionalTaxInfoOptions));
		services.Configure<RegionalTaxInfoFactory.RegionalTaxInfoOptions>(configurationSection);
		return services;
	}

	public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
	{
		// TODO:

		return services;
	}
}
