namespace Domain;

public record OrderLineItem(string SkuText, ushort UnitQuantity, decimal UnitPrice)
{
	public decimal TotalPrice => UnitPrice * UnitQuantity;
	public string SkuText { get; } = SkuText;
	public ushort UnitQuantity { get; } = UnitQuantity;
	public decimal UnitPrice { get; } = UnitPrice;
}