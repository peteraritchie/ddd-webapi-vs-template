using Application.Abstractions;
using Application.Services;
using Application.UseCases;
using Domain.Abstractions;
using Domain.Commands;
using Microsoft.Extensions.DependencyInjection;
using PRI.Messaging.Patterns;
using PRI.Messaging.Primitives;

namespace Application
{
	public static class ApplicationServiceCollectionExtensions
	{
		public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
		{
			var bus = new Bus();
			//bus.AddHandler(
			//    new ActionConsumer<CreateOrder>(c =>
			//        bus.Publish(new OrderCreated(Guid.Parse(c.CorrelationId),
			//            Guid.Empty,
			//            new Order(DateTime.UtcNow, c.OrderItems, c.ShippingAddress)))));
			services.AddSingleton<IBus>(bus);
			services.AddSingleton<IFundsTransferService, FundsTransferService>();
			services.AddSingleton<INotificationService, NotificationService>();
			services.AddSingleton<OrderService>();
			services.AddSingleton<AccountService>();
			services.AddSingleton<IConsumer<CreateOrder>, CreateOrderHandler>();

			using var serviceProvider = services.BuildServiceProvider();
			using var scope = serviceProvider.CreateScope();

			bus.AddHandler(scope.ServiceProvider.GetRequiredService<IConsumer<CreateOrder>>());
			return services;
		}
	}
}
