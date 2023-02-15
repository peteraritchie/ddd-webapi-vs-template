using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Dtos;

/// <summary>
/// Order
/// </summary>
[DisplayName("Order")]
public record OrderDto
{
    /// <summary>
    /// The date when the order was placed
    /// </summary>
    /// <example>2023-02-09T17:03:54.7629825+00:00</example>
    [Required]
    [JsonPropertyName("orderDate")]
    public DateTimeOffset? OrderDate { get; set; }

    /// <summary>
    /// Collection of order items
    /// </summary>
    [Required]
    [JsonPropertyName("orderItems")]
    public IEnumerable<OrderItemDto>? OrderItems { get; set; }

    /// <summary>
    /// Shipping address
    /// </summary>
    [Required]
    [JsonPropertyName("shippingAddress")]
    public PostalAddressDto? ShippingAddress { get; set; }

    /// <summary>
    /// Optional billing address
    /// </summary>
    [JsonPropertyName("billingAddress")]
    public PostalAddressDto? BillingAddress { get; set; }
}
