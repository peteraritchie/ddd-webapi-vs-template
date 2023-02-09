using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

[DisplayName("Order")]
public class OrderDto
{
    public OrderDto()
    {
        OrderDate = DateTime.UtcNow;
        OrderItems = new List<OrderItemDto>();
        ShippingAddress = new PostalAddressDto(string.Empty, string.Empty, string.Empty, string.Empty);
    }

    public OrderDto(DateTimeOffset orderDate, PostalAddressDto shippingAddress, IEnumerable<OrderItemDto> orderItems)
    {
        OrderDate = orderDate;
        OrderItems = orderItems;
        ShippingAddress = shippingAddress;
    }

    /// <summary>
    /// The date when the order was placed
    /// </summary>
    /// <example>2023-02-09T17:03:54.7629825+00:00</example>
    [Required]
    public DateTimeOffset OrderDate { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
    [Required]
    public PostalAddressDto ShippingAddress { get; set; }
    public PostalAddressDto? BillingAddress { get; set; }
}

[DisplayName("OrderItem")]
public class OrderItemDto
{
    public OrderItemDto()
    {
        SkuText = string.Empty;
        UnitQuantity = 1;
    }

    public OrderItemDto(string skuText, ushort unitQuantity, decimal unitPrice)
    {
        SkuText = skuText;
        UnitQuantity = unitQuantity;
        UnitPrice = unitPrice;
    }

    [Required]
    [StringLength(2)]
    public string SkuText { get; set; }
    [Required]
    public ushort UnitQuantity { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }
}

[DisplayName("PostalAddress")]
public class PostalAddressDto
{
    public PostalAddressDto(string streetAddress, string cityName, string stateName, string postalCode)
    {
        StreetAddress = streetAddress;
        CityName = cityName;
        StateName = stateName;
        PostalCode = postalCode;
    }

    public string? AttentionText { get; set; }
    [Required]
    public string StreetAddress { get; set; }
    public string? AlternateLocationText { get; set; }
    [Required]
    public string CityName { get; set; }
    [Required]
    public string StateName { get; set; }
    [Required]
    public string PostalCode { get; set; }
}
