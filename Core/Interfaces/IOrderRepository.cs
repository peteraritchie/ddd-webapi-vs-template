namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        public void Create(Guid orderId, Order order);
        public void Delete(Guid orderId);
        public Order Update(Guid orderId, Order order);
        public Order Get(Guid orderId);
    }
}
