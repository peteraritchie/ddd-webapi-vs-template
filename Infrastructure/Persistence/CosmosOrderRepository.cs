using Domain;
using Domain.Abstractions;
using Infrastructure.Dtos;
using Infrastructure.Dtos.Translators;
using Infrastructure.Services;

namespace Infrastructure.Persistence
{
	internal class CosmosOrderRepository : IOrderRepository
	{
		private readonly CosmosDbService<OrderDto> databaseService;

		public CosmosOrderRepository(CosmosDbService<OrderDto> databaseService)
		{
			this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
		}

		public async Task CreateAsync(Guid orderId, Order order)
		{
			ArgumentNullException.ThrowIfNull(order);

			var orderDto = OrderDtoTranslator.FromDomain(
				orderId,
				order);
			await databaseService.CreateAsync(orderDto);
		}

		public Task DeleteAsync(Guid orderId)
		{
			return databaseService.DeleteAsync(orderId.ToString());
		}

		public async Task<Order> UpdateAsync(Guid orderId, Order order)
		{
			var orderDto = OrderDtoTranslator.FromDomain(
				orderId,
				order);
			orderDto = await databaseService.UpsertAsync(orderDto);
			return orderDto.ToDomain();
		}

		public async Task<Order> GetAsync(Guid orderId)
		{
			var orderDto = await databaseService.GetAsync(orderId.ToString());
			return orderDto.ToDomain();
		}
	}
}
