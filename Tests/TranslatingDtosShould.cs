using WebApi.Dtos;
using WebApi.Dtos.Translators;

namespace Tests;

public sealed class TranslatingDtosShould
{
	[Fact]
	public void ValidOrderItemDtoShouldTranslateSuccessfully()
	{
		var orderItemDto = TestData.CreateOrderItem();
		var actual = orderItemDto.ToDomain();
		Assert.Equal(
			orderItemDto.SkuText,
			actual.SkuText);
		Assert.Equal(
			orderItemDto.UnitPrice,
			actual.UnitPrice);
		Assert.Equal(
			orderItemDto.UnitQuantity,
			actual.UnitQuantity);
	}

	[Fact]
	public void OrderItemDtoMissingQuantityShouldThrow()
	{
		var orderItem = TestData.CreateOrderItem();
		orderItem.UnitQuantity = default;

		Assert.Throws<ArgumentException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoMissingPriceShouldThrow()
	{
		var orderItem = TestData.CreateOrderItem();
		orderItem.UnitPrice = default;

		Assert.Throws<ArgumentException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoMissingSkuShouldThrow()
	{
		var orderItem = TestData.CreateOrderItem();
		orderItem.SkuText = default;

		Assert.Throws<ArgumentException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoZeroQuantityShouldThrow()
	{
		var orderItem = TestData.CreateOrderItem();
		orderItem.UnitQuantity = 0;

		Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoInvalidQuantityShouldThrow()
	{
		var orderItem = TestData.CreateOrderItem();
		orderItem.UnitQuantity = ushort.MaxValue;

		Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void ThrowWithNullAddress()
	{
		_ = Assert.Throws<ArgumentNullException>(() => default(PostalAddressDto).ToDomain());
	}

	[Fact]
	public void SucceedWithValidAddress()
	{
		var shippingAddressDto = TestData.CreateShippingAddress();
		var postalAddress = shippingAddressDto.ToDomain();
		Assert.NotNull(postalAddress);
		Assert.Equal(
			shippingAddressDto.AlternateLocationText,
			postalAddress.AlternateLocationText);
		Assert.Equal(
			shippingAddressDto.AttentionText,
			postalAddress.AttentionText);
		Assert.Equal(
			shippingAddressDto.PostalCodeText,
			postalAddress.PostalCodeText);
		Assert.Equal(
			shippingAddressDto.StateName,
			postalAddress.StateName);
		Assert.Equal(
			shippingAddressDto.StreetAddress,
			postalAddress.StreetAddress);
	}

	[Fact]
	public void ThrowsWithMissingStreetAddress()
	{
		var address = TestData.CreateShippingAddress();
		address.StreetAddress = default;

		Assert.Throws<ArgumentException>(() => address.ToDomain());
	}

	[Fact]
	public void ThrowsWithMissingCityName()
	{
		var address = TestData.CreateShippingAddress();
		address.CityName = default;

		Assert.Throws<ArgumentException>(() => address.ToDomain());
	}

	[Fact]
	public void ThrowsWithMissingStateName()
	{
		var address = TestData.CreateShippingAddress();
		address.StateName = default;

		Assert.Throws<ArgumentException>(() => address.ToDomain());
	}

	[Fact]
	public void ThrowsWithMissingPostalCodeText()
	{
		var address = TestData.CreateShippingAddress();
		address.PostalCodeText = default;

		Assert.Throws<ArgumentException>(() => address.ToDomain());
	}

	[Fact]
	public void SucceedWithMinimalOrder()
	{
		Assert.NotNull(TestData.CreateMinimalOrder().ToDomain());
	}

	[Fact]
	public void SucceedWithBillingAddressOrder()
	{
		Assert.NotNull(TestData.CreateOrder().ToDomain());
	}

	[Fact]
	public void ThrowWithMissingOrderDate()
	{
		var order = TestData.CreateMinimalOrder();
		order.OrderDate = default;

		Assert.Throws<ArgumentException>(order.ToDomain);
	}

	[Fact]
	public void ThrowWithMissingShippingAddress()
	{
		var order = TestData.CreateMinimalOrder();
		order.ShippingAddress = default;

		Assert.Throws<ArgumentException>(order.ToDomain);
	}

	[Fact]
	public void ThrowWithMissingOrderItems()
	{
		var order = TestData.CreateMinimalOrder();
		order.OrderItems = default;

		Assert.Throws<ArgumentException>(order.ToDomain);
	}
}