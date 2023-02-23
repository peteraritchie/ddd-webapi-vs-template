using Domain;
using Domain.Abstractions;

namespace WebApi.Infrastructure;

public sealed class InMemoryOrderRepository : InMemoryRepositoryBase<Order>, IOrderRepository
{
	public InMemoryOrderRepository()
	{
	}

	public InMemoryOrderRepository(Guid guid, Order order)
	{
		entityDictionary[guid] = order;
	}
}
