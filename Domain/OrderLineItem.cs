namespace Domain;

public record OrderLineItem(string SkuText, ushort UnitQuantity, decimal UnitPrice)
{
	public decimal TotalPrice => UnitPrice * UnitQuantity;
}
