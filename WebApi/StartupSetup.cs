using Domain;
using Domain.Abstractions;
using WebApi.Infrastructure;

namespace WebApi;

public static class StartupSetup
{
	public static IApplicationBuilder Configure(this IApplicationBuilder app)
	{
		return app;
	}

	/// <summary>
	///     Add services and types that are infrastructure-related
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection ConfigureServices(this IServiceCollection services)
	{
		services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
		services.AddSingleton<IAccountRepository>(
			sp => new InMemoryAccountRepository(
				new Dictionary<Guid, Account>
				{
					{
						Guid.Parse("478131a6-52fe-4dd6-9bc8-f2e01b34be6c"),
						new Account(
							new AccountHolder(
								string.Empty,
								string.Empty,
								string.Empty),
							10_000m)
					},
					{
						Guid.Parse("f5eb6500-4ab6-466c-b483-325d21e67344"),
						new Account(
							new AccountHolder(
								string.Empty,
								string.Empty,
								string.Empty),
							3_000m)
					}
				}));
		return services;
	}

	public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
	{
		return services;
	}
}