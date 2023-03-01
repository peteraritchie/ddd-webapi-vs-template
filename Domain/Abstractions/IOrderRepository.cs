namespace Domain.Abstractions
{
	public interface IOrderRepository
	{
		public Task CreateAsync(Guid orderId, Order order);
		public Task DeleteAsync(Guid orderId);
		public Task<Order> UpdateAsync(Guid orderId, Order order);
		public Task<Order> GetAsync(Guid orderId);
	}
}
