using Application.Abstractions;
using Domain.Abstractions;

namespace Application.Services
{
	/// <summary>
	///     Example application service from [Evans2004]
	/// </summary>
	internal class FundsTransferService : IFundsTransferService
	{
		private readonly IAccountRepository repository;

		public FundsTransferService(IAccountRepository repository)
		{
			this.repository = repository;
		}

		public async Task TransferAsync(Guid sourceAccountId, Guid destinationAccountId, decimal amount)
		{
			var sourceAccount = await repository.GetAsync(sourceAccountId).ConfigureAwait(false);
			var destinationAccount = await repository.GetAsync(destinationAccountId).ConfigureAwait(false);
			sourceAccount.Debit(amount);
			destinationAccount.Debit(amount);
			sourceAccount.Credit(amount);

			await repository.UpdateAsync(
				sourceAccountId,
				sourceAccount).ConfigureAwait(false);
			try
			{
				await repository.UpdateAsync(
					destinationAccountId,
					destinationAccount).ConfigureAwait(false);
			}
			catch (Exception)
			{
				sourceAccount.Credit(amount);
				await repository.UpdateAsync(
					sourceAccountId,
					sourceAccount).ConfigureAwait(false);
				throw;
			}
		}
	}
}
