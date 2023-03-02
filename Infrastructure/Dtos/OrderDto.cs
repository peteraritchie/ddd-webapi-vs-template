using Infrastructure.Common;

namespace Infrastructure.Dtos;

public class OrderDto : IIdentifiable
{
	public DateTime? DateTime { get; set; }
	public IEnumerable<OrderItemDto>? OrderItems { get; set; }
	public PostalAddressDto? ShippingAddress { get; set; }
	public PostalAddressDto? BillingAddress { get; set; }
	public string? Id { get; set; }
}
