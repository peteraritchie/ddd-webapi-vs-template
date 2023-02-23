namespace Domain.Builders;

public class OrderBuilder
{
	private readonly List<OrderLineItem> lineItems;
	private PostalAddress? billingAddress;
	private DateTime? dateTime;
	private PostalAddress? shippingAddress;

	public OrderBuilder()
	{
		lineItems = new List<OrderLineItem>();
	}

	public void Validate()
	{
		if (!dateTime.HasValue)
		{
			throw new InvalidOperationException();
		}

		if (shippingAddress == null)
		{
			throw new InvalidOperationException();
		}

		if (!lineItems.Any())
		{
			throw new InvalidOperationException();
		}
	}

	public OrderBuilder At(DateTime dateTime)
	{
		this.dateTime = dateTime;
		return this;
	}

	public OrderBuilder ShippingTo(PostalAddress address)
	{
		shippingAddress = address;
		return this;
	}

	public OrderBuilder BillingTo(PostalAddress address)
	{
		billingAddress = address;
		return this;
	}

	public OrderBuilder BillToShippingAddress()
	{
		billingAddress = shippingAddress;
		return this;
	}

	public OrderBuilder WithProduct(string skuText, decimal price, int quantity)
	{
		if (quantity is < 1 or > ushort.MaxValue)
		{
			throw new ArgumentOutOfRangeException(nameof(quantity), quantity,
				$"Quantity should be greater than 0 and less than {ushort.MaxValue}");
		}

		lineItems.Add(new OrderLineItem(skuText, (ushort)quantity, price));

		return this;
	}

	public Order Build()
	{
		Validate();

		var result = new Order(dateTime!.Value, lineItems, shippingAddress!, billingAddress);

		return result;
	}
}
