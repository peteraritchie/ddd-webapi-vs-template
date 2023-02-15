using System.Net;
using Core.Commands;
using Core.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using PRI.Messaging.Patterns.Extensions.Bus;
using PRI.Messaging.Primitives;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Common;
using WebApi.Dtos;
using WebApi.Dtos.Translators;

namespace WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IBus bus;
    private readonly OrdersControllerOptions options;

    public OrdersController(IOptions<OrdersControllerOptions> options, IBus bus)
    {
        this.bus = bus;
        this.options = options.Value;
    }

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
    [Consumes(typeof(OrderDto), ApplicationContentTypes.OrderJson)]
    [Produces(ApplicationContentTypes.OrderJson, ModernMediaTypeNames.Application.ProblemJson)] // required along with ProducesResponseType
    [ProducesResponseType(typeof(OrderDto),
        statusCode: StatusCodes.Status201Created,
        contentType: ApplicationContentTypes.OrderJson)]
    [ProducesResponseType(typeof(ProblemDetails),
        statusCode: StatusCodes.Status400BadRequest,
        contentType: ModernMediaTypeNames.Application.ProblemJson)]
    [SwaggerResponseHeader(StatusCodes.Status201Created,
        nameof(HttpResponseHeader.Location),
        type: "string",
        description: "Location of the newly created resource")]
    public async Task<IActionResult> CreateOrderAsync(OrderDto order)
    {
        var correlationId = DiscoverCorrelationId();

        var createOrder = new CreateOrder(correlationId, order.OrderDate!.Value.DateTime,
            order.OrderItems!.Select(OrderDtoTranslator.ToDomain),
            shippingAddress: OrderDtoTranslator.ToDomain(order.ShippingAddress!),
            billingAddress: order.BillingAddress != null ? OrderDtoTranslator.ToDomain(order.BillingAddress) : default);

        var orderCreated = await bus.RequestAsync<CreateOrder, OrderCreated>(createOrder);

        return CreatedAtAction(nameof(GetOrder),
            routeValues: new { orderCreated.OrderId},
            value: OrderDtoTranslator.FromDomain(orderCreated.Order));
    }

    private Guid DiscoverCorrelationId()
    {
        if (!Request.Headers.TryGetValue(CorrelationIdKeyName, out var values))
        {
            if (!Request.Headers.TryGetValue(HeaderNames.RequestId, out values))
            {
            }
        }

        var headerValue = values.FirstOrDefault();
        if (!Guid.TryParse(headerValue, out var correlationId))
            correlationId = Guid.NewGuid();

        Response.Headers.Add(CorrelationIdKeyName, correlationId.ToString());

        return correlationId;
    }

    public const string CorrelationIdKeyName = "Correlation-ID";

    private static OrderDto GenerateExampleOrder()
    {
        return new OrderDto
        {
            OrderDate = DateTimeOffset.UtcNow,
            ShippingAddress = new PostalAddressDto
            {
                StreetAddress = "14544 ROGUE RIVER DR",
                CityName = "CHESTERFIELD",
                StateName = "MO",
                PostalCodeText = "63017"
            },
            OrderItems = new[]
            {
                new OrderItemDto()
            }
        };
    }

    /// <summary>
    /// Example controller options
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "In progress.")]
    public class OrdersControllerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public bool FeatureFlag1 { get; set; }
    }
}
