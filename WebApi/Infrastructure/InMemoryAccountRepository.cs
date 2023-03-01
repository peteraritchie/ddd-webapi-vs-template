using Domain;
using Domain.Abstractions;
using PRI.ProductivityExtensions.ICollectionableExtensions;

namespace WebApi.Infrastructure
{
	public class InMemoryAccountRepository : InMemoryRepositoryBase<Account>, IAccountRepository
	{
		public InMemoryAccountRepository()
		{
		}

		public InMemoryAccountRepository(Guid guid, Account account)
		{
			entityDictionary[guid] = account;
		}

		public InMemoryAccountRepository(IDictionary<Guid, Account> accounts)
		{
			entityDictionary.AddRange(accounts);
		}
	}
}
