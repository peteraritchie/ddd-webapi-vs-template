using Domain.Commands;
using Microsoft.Extensions.DependencyInjection;
using PRI.Messaging.Patterns;
using PRI.Messaging.Primitives;

namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
	public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
	{
		var bus = new Bus();
		//bus.AddHandler(
		//    new ActionConsumer<CreateOrder>(c =>
		//        bus.Publish(new OrderCreated(Guid.Parse(c.CorrelationId),
		//            Guid.Empty,
		//            new Order(DateTime.UtcNow, c.OrderItems, c.ShippingAddress)))));
		services.AddSingleton<IBus>(bus);
		services.AddSingleton<IConsumer<CreateOrder>, CreateOrderHandler>();

		using var serviceProvider = services.BuildServiceProvider();
		using var scope = serviceProvider.CreateScope();

		bus.AddHandler(scope.ServiceProvider.GetRequiredService<IConsumer<CreateOrder>>());

		return services;
	}

	public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
	{
		// TODO:

		return services;
	}
}
