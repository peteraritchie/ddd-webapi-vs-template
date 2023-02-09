using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Examples;
using WebApi.Common;
using WebApi.Dtos;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    /// <summary>
    /// Get a order by id
    /// </summary>
    /// <param name="orderId" example="76078b26-6130-4b93-9c26-3a37616e1244">The order id to obtain.</param>
    /// <returns>An <seealso cref="OrderDto"/></returns>
    [HttpGet]
    [Produces(ApplicationContentTypes.OrderJson, ModernMediaTypeNames.Application.ProblemJson)] // required along with ProducesResponseType
    [ProducesResponseType(typeof(OrderDto),
        statusCode: StatusCodes.Status200OK,
        contentType: ApplicationContentTypes.OrderJson)]
    [ProducesResponseType(typeof(ProblemDetails),
        statusCode: StatusCodes.Status400BadRequest,
        contentType: ModernMediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType(typeof(ProblemDetails),
        statusCode: StatusCodes.Status500InternalServerError,
        contentType: ModernMediaTypeNames.Application.ProblemJson)]
    [Route("{orderId:guid}")]
    public Task<ActionResult<OrderDto>> GetOrder([FromRoute]Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            return Task.FromResult<ActionResult<OrderDto>>(ValidationProblem("invalid id", statusCode: StatusCodes.Status400BadRequest));
        }

        return Task.FromResult<ActionResult<OrderDto>>(Ok(GenerateExampleOrder()));
    }

    /// <summary>Create an order</summary>
    [HttpPost]
    [Produces(ApplicationContentTypes.OrderJson, ModernMediaTypeNames.Application.ProblemJson)] // required along with ProducesResponseType
    [ProducesResponseType(typeof(OrderDto),
        statusCode: StatusCodes.Status201Created,
        contentType: ApplicationContentTypes.OrderJson)]
    [ProducesResponseType(typeof(ProblemDetails),
        statusCode: StatusCodes.Status400BadRequest,
        contentType: ModernMediaTypeNames.Application.ProblemJson)]
    [SwaggerResponseHeader(HttpStatusCode.Created,
        nameof(HttpResponseHeader.Location),
        type: "string",
        description: "Location of the newly created resource")]
    public async Task<IActionResult> CreateOrderAsync(OrderDto order)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var orderId = Guid.NewGuid();

        return CreatedAtAction(nameof(GetOrder),
                routeValues: new { orderId },
                value: order);
    }

    //[HttpGet]
    //public IActionResult GetCreateOrderRequest([FromRoute]Guid requestId)
    //{
    //    return Ok(new { status = "pending" });
    //}

    private static OrderDto GenerateExampleOrder()
    {
        return new OrderDto(DateTimeOffset.UtcNow, new PostalAddressDto("14544 ROGUE RIVER DR", "CHESTERFIELD", "MO", "63017"), new []
        {
            new OrderItemDto()
        });
    }
}

public static class ApplicationContentTypes
{
    public const string OrderJson = "application/vnd.contoso.sales.order+json; charset=utf-8; version=1.0";
    public const string AcceptedJson = "application/vnd.contoso.accepted+json; charset=utf-8";
}

public class AcceptedResponse
{
}
