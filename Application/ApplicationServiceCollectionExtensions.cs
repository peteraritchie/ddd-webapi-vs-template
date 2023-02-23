using Application.Abstractions;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceCollectionExtensions
{
	public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
	{
		services.AddSingleton<IFundsTransferService, FundsTransferService>();
		return services;
	}
}
