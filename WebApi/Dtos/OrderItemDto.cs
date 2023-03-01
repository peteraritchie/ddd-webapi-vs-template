using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Dtos
{
	/// <summary>
	///     OrderItem
	/// </summary>
	[DisplayName("OrderItem")]
	public record OrderItemDto
	{
		/// <summary>
		///     The SKU of the purchased item
		/// </summary>
		/// <example>abc123</example>
		[Required]
		[StringLength(20)]
		[JsonPropertyName("sku")]
		public string? SkuText { get; set; }

		/// <summary>
		///     The quantity of items
		/// </summary>
		/// <example>1</example>
		[Required]
		[Range(
			1,
			999)]
		[JsonPropertyName("qty")]
		public ushort? UnitQuantity { get; set; }

		/// <summary>
		///     The price of the item
		/// </summary>
		/// <example>9.99</example>
		[Required]
		[Range(
			0,
			1_000_000)]
		[JsonPropertyName("price")]
		public decimal? UnitPrice { get; set; }
	}
}
