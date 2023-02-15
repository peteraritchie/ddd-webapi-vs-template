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
    public async Task ReturnValidationProblemDetailsWhenModelIsNotValid()
    {
        controller.ModelState.AddModelError("one", "message");

        var result = await controller.CreateOrderAsync(TestData.Order);

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
        var mockBus = new Mock<IBus>();
        var options =
            Options.Create(new OrdersController.OrdersControllerOptions());
        var controller = new OrdersController(options, mockBus.Object);
        return controller;
    }
}
