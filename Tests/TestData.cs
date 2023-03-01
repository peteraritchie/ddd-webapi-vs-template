using WebApi.Dtos;

namespace Tests;

public static class TestData
{
	public const string OrderJsonText =
		"{\"orderDate\":\"2023-02-10T02:41:09.005+00:00\",\"orderItems\":[{\"sku\":\"abc123\",\"qty\":2,\"price\":11.50}],\"shippingAddress\":{\"attentionText\":null,\"streetAddress\":\"14544 ROGUE RIVER DR\",\"alternateLocationText\":null,\"cityName\":\"CHESTERFIELD\",\"stateName\":\"MO\",\"postalCode\":\"63017\"},\"billingAddress\":null}";

	public static readonly DateTimeOffset OrderDate = new(new DateTime(2023, 02, 10, 2, 41, 9, 5),
		TimeSpan.FromHours(0));
	//public static readonly DateTimeOffset OrderDate = DateTime.SpecifyKind(new DateTime(2023, 02, 10, 2, 41, 9, 5), DateTimeKind.Utc);

	public static PostalAddressDto CreateShippingAddress()
	{
		return new PostalAddressDto
		{
			StreetAddress = "14544 ROGUE RIVER DR",
			CityName = "CHESTERFIELD",
			StateName = "MO",
			PostalCodeText = "63017"
		};
	}

	public static IEnumerable<OrderItemDto> CreateOrderItems() => new[]
	{
		CreateOrderItem()
	};

	public static OrderItemDto CreateOrderItem()
	{
		return new OrderItemDto
		{
			SkuText = "abc123",
			UnitQuantity = 2,
			UnitPrice = 11.50m
		};
	}

	public static OrderDto CreateMinimalOrder() => new()
	{
		OrderDate = OrderDate,
		ShippingAddress = CreateShippingAddress(),
		OrderItems = CreateOrderItems()
	};

	public static OrderDto CreateOrder()
	{
		return new OrderDto
		{
			OrderDate = OrderDate,
			ShippingAddress = CreateShippingAddress(),
			BillingAddress = CreateShippingAddress(),
			OrderItems = new[] { CreateOrderItem() }
		};
	}
}
