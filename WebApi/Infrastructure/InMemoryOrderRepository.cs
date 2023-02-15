using Core;
using Core.Interfaces;
using Core.Exceptions;

namespace WebApi.Infrastructure
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        readonly Dictionary<Guid, Order> orderDictionary = new ();

        public void Create(Guid orderId, Order order)
        {
            if (!orderDictionary.ContainsKey(orderId)) throw new EntityAlreadyExistsException(orderId);
            orderDictionary[orderId] = order;
        }

        public void Delete(Guid orderId)
        {
            if (!orderDictionary.ContainsKey(orderId)) throw new EntityNotFoundException(orderId);
            orderDictionary.Remove(orderId);
        }

        public Order Update(Guid orderId, Order order)
        {
            if (!orderDictionary.ContainsKey(orderId)) throw new EntityNotFoundException(orderId);
            orderDictionary[orderId] = order;
            // TODO: merge orders?
            return order;
        }

        public Order Get(Guid orderId)
        {
            if (!orderDictionary.ContainsKey(orderId)) throw new EntityNotFoundException(orderId);
            return orderDictionary[orderId];
        }
    }
}
