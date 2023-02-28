using Domain;
using Domain.Exceptions;
using WebApi.Infrastructure;

namespace Tests;

public class InMemoryAccountRepositoryShould
{
	private static readonly Guid defaultGuid = Guid.NewGuid();
	private readonly Account stubAccount = new(new AccountHolder(string.Empty, string.Empty, string.Empty), 100m);
	private readonly InMemoryAccountRepository repository;

	public InMemoryAccountRepositoryShould()
	{
		repository = new InMemoryAccountRepository(defaultGuid, stubAccount);
	}

	[Fact]
	public async Task CreateSuccessfully()
	{
		var guid = Guid.NewGuid();
		await repository.CreateAsync(guid, stubAccount);
		var account = await repository.GetAsync(guid);
		Assert.NotNull(account);
		Assert.Equal(100m, account.Balance);
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
		var account = await repository.GetAsync(defaultGuid);
		Assert.Equal(100m, account.Balance);
		await repository.UpdateAsync(defaultGuid, stubAccount);
		account = await repository.GetAsync(defaultGuid);
		Assert.Equal(100m, account.Balance);
	}

	[Fact]
	public async Task ThrowCreatingExisting()
	{
		await Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await repository.CreateAsync(defaultGuid, stubAccount));
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
		await Assert.ThrowsAsync<EntityNotFoundException>(async () => await repository.UpdateAsync(defaultGuid, stubAccount));
	}
}
