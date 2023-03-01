using Domain;

namespace Tests.Domain
{
	public class OrderShould
	{
		[Fact]
		public void AddFirstOrderItemCorrectly()
		{
			var order = new Order(
				DomainTestData.OrderDate,
				Array.Empty<OrderLineItem>(),
				DomainTestData.CreateShippingAddress());
			Assert.Empty(order.OrderItems);
			order.AddItem(DomainTestData.CreateOrderItem());
			Assert.Single(order.OrderItems);
		}

		[Fact]
		public void AddFirstOrderItemResultsInCorrectItem()
		{
			var order = new Order(
				DomainTestData.OrderDate,
				Array.Empty<OrderLineItem>(),
				DomainTestData.CreateShippingAddress());
			var newOrderLineItem = DomainTestData.CreateOrderItem();
			order.AddItem(newOrderLineItem);
			var actual = order.OrderItems.Single();
			Assert.Equal(
				newOrderLineItem.SkuText,
				actual.SkuText);
			Assert.Equal(
				newOrderLineItem.UnitQuantity,
				actual.UnitQuantity);
			Assert.Equal(
				newOrderLineItem.UnitPrice,
				actual.UnitPrice);
		}

		[Fact]
		public void AddOrderItemResultsInCorrectItem()
		{
			var order = new Order(
				DomainTestData.OrderDate,
				new[] { DomainTestData.CreateOrderItem() },
				DomainTestData.CreateShippingAddress());
			order.AddItem(
				"skuText2",
				2,
				19.99m);
			var actual = order.OrderItems.Last();
			Assert.Equal(
				"skuText2",
				actual.SkuText);
			Assert.Equal(
				2,
				actual.UnitQuantity);
			Assert.Equal(
				19.99m,
				actual.UnitPrice);
		}

		[Fact]
		public void AddOrderItemCorrectly()
		{
			var order = new Order(
				DomainTestData.OrderDate,
				new[] { DomainTestData.CreateOrderItem() },
				DomainTestData.CreateShippingAddress());
			Assert.Single(order.OrderItems);
			order.AddItem(
				"skuText2",
				2,
				19.99m);
			Assert.Equal(
				2,
				order.OrderItems.Count());
		}
	}
}
