using Domain;

namespace Tests.Domain
{
	public static class DomainTestData
	{
		public static readonly DateTime OrderDate = DateTime.SpecifyKind(
			new DateTime(
				2023,
				02,
				10,
				2,
				41,
				9,
				5),
			DateTimeKind.Utc);

		public static PostalAddress CreateShippingAddress()
		{
			return new PostalAddress(
				"14544 ROGUE RIVER DR",
				"CHESTERFIELD",
				"MO",
				"63017"
			);
		}

		public static OrderLineItem CreateOrderItem()
		{
			return new OrderLineItem(
				"skuText",
				1,
				9.99m);
		}
	}
}
