namespace Domain.Abstractions;

public interface IAccountRepository
{
	public Task CreateAsync(Guid accountId, Account order);
	public Task DeleteAsync(Guid accountId);

	Task<Account> GetAsync(Guid accountId);
	Task<Account> UpdateAsync(Guid accountId, Account account);
}
