using PRI.Messaging.Primitives;

namespace Core.Commands
{
    public class AddOrderItem : ICommand
    {
        public AddOrderItem(string correlationId, Guid orderId, string sku, ushort quantity, decimal price)
        {
            CorrelationId = correlationId;
            OrderId = orderId;
            SkuText = sku;
            UnitQuantity = quantity;
            UnitPrice = price;
        }

        public string CorrelationId { get; set; }
        public string SkuText { get; }
        public ushort UnitQuantity { get; }
        public decimal UnitPrice { get; }
        public Guid OrderId { get; set; }
    }
}
