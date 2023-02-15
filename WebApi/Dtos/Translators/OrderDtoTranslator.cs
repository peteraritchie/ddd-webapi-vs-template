using Core;
using Core.Builders;

namespace WebApi.Dtos.Translators;

public static class OrderDtoTranslator
{
    public static Order ToDomain(OrderDto order)
    {
        if(!order.OrderDate.HasValue) throw new ArgumentException($"{nameof(order)}.{nameof(order.OrderDate)} is required.", nameof(order));
        if(order.ShippingAddress is null) throw new ArgumentException($"{nameof(order)}.{nameof(order.ShippingAddress)} is required.", nameof(order));
        if (order.OrderItems == null) throw new ArgumentException($"{nameof(order)}.{nameof(order.OrderItems)} is required.", nameof(order));

        var builder = new OrderBuilder()
            .At(order.OrderDate.Value.DateTime)
            .ShippingTo(order.ShippingAddress.ToDomain());

        if(order.BillingAddress is not null)
        {
            builder.BillingTo(order.BillingAddress.ToDomain());
        }

        foreach (var p in order.OrderItems)
        {
            var domain = p.ToDomain();
            builder.WithProduct(domain.SkuText, domain.UnitPrice, domain.UnitQuantity);
        }

        return builder.Build();
    }

    public static OrderDto FromDomain(Order order)
    {
        var result = new OrderDto
        {
            OrderDate = order.DateTime,
            OrderItems = order.OrderItems.Select(i=>i.FromDomain()),
            ShippingAddress = order.ShippingAddress.FromDomain(),
            BillingAddress = order?.BillingAddress?.FromDomain()
        };
        return result;
    }

    public static OrderLineItem ToDomain(OrderItemDto item)
    {
        return item.ToDomain();
    }

    public static PostalAddress ToDomain(PostalAddressDto address)
    {
        return address.ToDomain();
    }
}
