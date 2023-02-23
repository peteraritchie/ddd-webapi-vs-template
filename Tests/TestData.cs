using WebApi.Dtos;

namespace Tests;

public static class TestData
{
	public const string OrderJsonText =
		"{\"orderDate\":\"2023-02-10T02:41:09.005+00:00\",\"orderItems\":[{\"sku\":\"abc123\",\"qty\":2,\"price\":11.50}],\"shippingAddress\":{\"attentionText\":null,\"streetAddress\":\"14544 ROGUE RIVER DR\",\"alternateLocationText\":null,\"cityName\":\"CHESTERFIELD\",\"stateName\":\"MO\",\"postalCode\":\"63017\"},\"billingAddress\":null}";

	public static readonly DateTimeOffset OrderDate = new(new DateTime(2023, 02, 10, 2, 41, 9, 5),
		TimeSpan.FromHours(0));
	//public static readonly DateTimeOffset OrderDate = DateTime.SpecifyKind(new DateTime(2023, 02, 10, 2, 41, 9, 5), DateTimeKind.Utc);

	public static readonly PostalAddressDto ShippingAddress = new()
	{
		StreetAddress = "14544 ROGUE RIVER DR",
		CityName = "CHESTERFIELD",
		StateName = "MO",
		PostalCodeText = "63017"
	};

	public static readonly OrderItemDto OrderItem = new()
	{
		SkuText = "abc123",
		UnitQuantity = 2,
		UnitPrice = 11.50m
	};

	public static OrderDto MinimalOrder => new()
	{
		OrderDate = OrderDate,
		ShippingAddress = ShippingAddress,
		OrderItems = new[] { OrderItem }
	};
	public static OrderDto Order => new()
	{
		OrderDate = OrderDate,
		ShippingAddress = ShippingAddress,
		BillingAddress = ShippingAddress,
		OrderItems = new[] { OrderItem }
	};
}
