namespace Core;

public class Order
{
    private readonly List<OrderLineItem> orderItems;
    public Order(DateTime dateTime, IEnumerable<OrderLineItem> orderItems, PostalAddress shippingAddress)
        : this(dateTime, orderItems, shippingAddress, default)
    {
    }

    public Order(DateTime dateTime, IEnumerable<OrderLineItem> orderItems, PostalAddress shippingAddress, PostalAddress? billingAddress)
    {
        DateTime = dateTime;
        this.orderItems = new List<OrderLineItem>(orderItems);
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
    }

    public DateTime DateTime { get; }
    public IEnumerable<OrderLineItem> OrderItems => orderItems.AsReadOnly();
    public PostalAddress ShippingAddress { get; }
    public PostalAddress? BillingAddress { get; }

    public void AddItem(string sku, ushort quantity, decimal price)
    {
        orderItems.Add(new OrderLineItem(sku, quantity, price));
    }
}
