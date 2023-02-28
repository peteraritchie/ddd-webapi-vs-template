using Domain.Exceptions;

using WebApi.Dtos.Translators;
using WebApi.Infrastructure;

namespace Tests;

public class InMemoryOrderRepositoryShould
{
	private static readonly Guid defaultGuid = Guid.NewGuid();

	private readonly InMemoryOrderRepository repository =
		new(defaultGuid, TestData.CreateMinimalOrder().ToDomain());

	[Fact]
	public async Task CreateSuccessfully()
	{
		var guid = Guid.NewGuid();
		await repository.CreateAsync(guid, TestData.CreateOrder().ToDomain());
		var order = await repository.GetAsync(guid);
		Assert.NotNull(order);
		Assert.NotNull(order.BillingAddress);
	}

	[Fact]
	public async Task GetSuccessfully()
	{
		Assert.NotNull(await repository.GetAsync(defaultGuid));
	}

	[Fact]
	public async Task DeleteSuccessfully()
	{
		await repository.DeleteAsync(defaultGuid);
		await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.GetAsync(defaultGuid));
	}

	[Fact]
	public async Task UpdateSuccessfully()
	{
		var order = await repository.GetAsync(defaultGuid);
		Assert.Null(order.BillingAddress);
		await repository.UpdateAsync(defaultGuid, TestData.CreateOrder().ToDomain());
		order = await repository.GetAsync(defaultGuid);
		Assert.NotNull(order.BillingAddress);
	}

	[Fact]
	public async Task ThrowCreatingExisting()
	{
		await Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await repository.CreateAsync(defaultGuid, TestData.CreateOrder().ToDomain()));
	}

	[Fact]
	public async Task ThrowDeletingNonExisting()
	{
		await repository.DeleteAsync(defaultGuid);
		await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.DeleteAsync(defaultGuid));
	}

	[Fact]
	public async Task ThrowUpdatingNonExisting()
	{
		await repository.DeleteAsync(defaultGuid);
		await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.UpdateAsync(defaultGuid, TestData.CreateOrder().ToDomain()));
	}
}
