namespace Infrastructure.Dtos
{
	public record OrderItemDto
	{
		public string? SkuText { get; set; }
		public int? Quantity { get; set; }
		public decimal? Price { get; set; }
	}
}
