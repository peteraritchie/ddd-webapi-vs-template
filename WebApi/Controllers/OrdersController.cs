using System.Diagnostics.CodeAnalysis;
using System.Net;
using Domain.Commands;
using Domain.Events;
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
///     The orders ControllerBase adapter
/// </summary>
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
	public const string CorrelationIdKeyName = "Correlation-ID";
	private readonly IBus bus;
	private readonly OrdersControllerOptions options;

	public OrdersController(IOptions<OrdersControllerOptions> options, IBus bus)
	{
		this.bus = bus;
		this.options = options.Value;
	}

	/// <summary>
	///     Get a order by id
	/// </summary>
	/// <param name="orderId" example="76078b26-6130-4b93-9c26-3a37616e1244">The order id to obtain.</param>
	/// <returns>An <seealso cref="OrderDto" /></returns>
	[HttpGet]
	[Produces(
		ApplicationContentTypes.OrderJson,
		ModernMediaTypeNames.Application.ProblemJson)] // required along with ProducesResponseType
	[ProducesResponseType(
		typeof(OrderDto),
		StatusCodes.Status200OK,
		ApplicationContentTypes.OrderJson)]
	[ProducesResponseType(
		typeof(ProblemDetails),
		StatusCodes.Status400BadRequest,
		ModernMediaTypeNames.Application.ProblemJson)]
	[ProducesResponseType(
		typeof(ProblemDetails),
		StatusCodes.Status500InternalServerError,
		ModernMediaTypeNames.Application.ProblemJson)]
	[Route("{orderId:guid}")]
	public Task<ActionResult<OrderDto>> GetOrder([FromRoute] Guid orderId)
	{
		if (orderId == Guid.Empty)
		{
			return Task.FromResult<ActionResult<OrderDto>>(
				ValidationProblem(
					"invalid id",
					statusCode: StatusCodes.Status400BadRequest));
		}

		return Task.FromResult<ActionResult<OrderDto>>(Ok(GenerateExampleOrder()));
	}

	/// <summary>Create an order</summary>
	[HttpPost]
	[Consumes(
		typeof(OrderDto),
		ApplicationContentTypes.OrderJson)]
	[Produces(
		ApplicationContentTypes.OrderJson,
		ModernMediaTypeNames.Application.ProblemJson)] // required along with ProducesResponseType
	[ProducesResponseType(
		typeof(OrderDto),
		StatusCodes.Status201Created,
		ApplicationContentTypes.OrderJson)]
	[ProducesResponseType(
		typeof(ProblemDetails),
		StatusCodes.Status400BadRequest,
		ModernMediaTypeNames.Application.ProblemJson)]
	[SwaggerResponseHeader(
		StatusCodes.Status201Created,
		nameof(HttpResponseHeader.Location),
		"string",
		"Location of the newly created resource")]
	public async Task<IActionResult> CreateOrderAsync(OrderDto order)
	{
		if (!ModelState.IsValid)
		{
			return ValidationProblem(ModelState);
		}

		var correlationId = DiscoverCorrelationId();

		var domainOrder = order.ToDomain();

		var createOrder = new CreateOrder(
			correlationId,
			domainOrder);

		var orderCreated = await bus.RequestAsync<CreateOrder, OrderCreated>(createOrder);

		return CreatedAtAction(
			nameof(GetOrder),
			new { orderCreated.OrderId },
			orderCreated.Order.FromDomain());
	}

	private Guid DiscoverCorrelationId()
	{
		if (!Request.Headers.TryGetValue(
			    CorrelationIdKeyName,
			    out var values))
		{
			if (!Request.Headers.TryGetValue(
				    HeaderNames.RequestId,
				    out values))
			{
			}
		}

		var headerValue = values.FirstOrDefault();
		if (!Guid.TryParse(
			    headerValue,
			    out var correlationId))
		{
			correlationId = Guid.NewGuid();
		}

		Response.Headers.Add(
			CorrelationIdKeyName,
			correlationId.ToString());

		return correlationId;
	}

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
	///     Example controller options
	/// </summary>
	[ExcludeFromCodeCoverage(Justification = "In progress.")]
	public class OrdersControllerOptions
	{
		/// <summary>
		/// </summary>
		public bool FeatureFlag1 { get; set; }
	}
}