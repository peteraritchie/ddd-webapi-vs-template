using System.Text.Json;
using WebApi.Dtos;

namespace Tests;

public sealed class OrderDtoSerializationShould
{
	[Fact]
	public void SerializeSuccessfully()
	{
		DateTimeOffset orderDate = new(new DateTime(2023, 02, 10, 2, 41, 9, 5), TimeSpan.FromHours(0));
		var shippingAddress = new PostalAddressDto
		{
			StreetAddress = "14544 ROGUE RIVER DR",
			CityName = "CHESTERFIELD",
			StateName = "MO",
			PostalCodeText = "63017"
		};
		var orderItem = new OrderItemDto
		{
			SkuText = "abc123",
			UnitQuantity = 2,
			UnitPrice = 11.50m
		};
		var order = new OrderDto
		{
			OrderDate = orderDate,
			ShippingAddress = shippingAddress,
			OrderItems = new[] { orderItem }
		};
		var jsonText = JsonSerializer.Serialize(order);

		Assert.Equal(TestData.OrderJsonText, jsonText);
	}

	[Fact]
	public void DeserializeSuccessfully()
	{
		var actualOrder = JsonSerializer.Deserialize<OrderDto>(TestData.OrderJsonText);
		Assert.NotNull(actualOrder);
		DateTimeOffset orderDate = new(new DateTime(2023, 02, 10, 2, 41, 9, 5), TimeSpan.FromHours(0));
		var shippingAddress = new PostalAddressDto
		{
			StreetAddress = "14544 ROGUE RIVER DR",
			CityName = "CHESTERFIELD",
			StateName = "MO",
			PostalCodeText = "63017"
		};
		var orderItem = new OrderItemDto
		{
			SkuText = "abc123",
			UnitQuantity = 2,
			UnitPrice = 11.50m
		};

		Assert.Equal(orderDate, actualOrder.OrderDate);
		Assert.Equal(shippingAddress, actualOrder.ShippingAddress);
		Assert.Null(actualOrder.BillingAddress);
		Assert.NotNull(actualOrder.OrderItems);
		var actualOrderItem = Assert.Single(actualOrder.OrderItems);
		Assert.Equal(orderItem, actualOrderItem);
	}
}
