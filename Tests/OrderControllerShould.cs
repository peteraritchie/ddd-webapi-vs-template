using Domain;
using Domain.Commands;
using Domain.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PRI.Messaging.Primitives;
using WebApi.Controllers;

namespace Tests;

public class OrderControllerShould
{
	private readonly OrdersController controller;

	public OrderControllerShould()
	{
		controller = CreateController();
	}

	[Fact]
	public async Task Test()
	{
		var result = await controller.CreateOrderAsync(TestData.Order);

		Assert.NotNull(result);
	}


	[Fact]
	public async Task ReturnValidationProblemDetailsWhenModelIsNotValid()
	{
		controller.ModelState.AddModelError("one", "message");

		var result = await controller.CreateOrderAsync(TestData.MinimalOrder);

		Assert.NotNull(result);
		var objectResult = Assert.IsType<ObjectResult>(result);
		var problemDetails = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
		Assert.NotNull(problemDetails);
		var error = Assert.Single(problemDetails.Errors);
		Assert.Equal("one", error.Key);
		var message = Assert.Single(error.Value);
		Assert.Equal("message", message);
	}

	private static OrdersController CreateController()
	{
		var httpContext = new DefaultHttpContext();
		httpContext.Request.Headers["Correlation-ID"] = Guid.NewGuid().ToString();
		var mockBus = new Mock<IBus>();
		IConsumer<OrderCreated>? consumer = default;
		mockBus
			.Setup(m => m.AddHandler(It.IsAny<IConsumer<OrderCreated>>()))
			.Callback((IConsumer<OrderCreated> c) => { consumer = c; })
			.Returns(() => new object());

		mockBus
			.Setup(m => m.Handle(It.IsAny<IMessage>()))
			.Callback<IMessage>(m =>
			{
				var co = (CreateOrder)m;
				consumer?.Handle(
					new OrderCreated(
						Guid.Parse(m.CorrelationId),
						Guid.NewGuid(),
						new Order(DateTime.UtcNow, co.OrderItems, co.ShippingAddress)));
			});
		var options =
			Options.Create(new OrdersController.OrdersControllerOptions());
		var controller = new OrdersController(options, mockBus.Object)
		{
			ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			}
		};
		return controller;
	}
}
