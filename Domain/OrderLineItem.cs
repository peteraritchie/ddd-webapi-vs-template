namespace Domain;

public class OrderLineItem
{
	public OrderLineItem(string sku, ushort quantity, decimal price)
	{
		SkuText = sku;
		UnitQuantity = quantity;
		UnitPrice = price;
	}

	public string SkuText { get; }
	public ushort UnitQuantity { get; }
	public decimal UnitPrice { get; }
	public decimal TotalPrice => UnitPrice * UnitQuantity;
}
