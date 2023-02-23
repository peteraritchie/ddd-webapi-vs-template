namespace Application.Abstractions;

public interface IFundsTransferService
{
	Task TransferAsync(Guid sourceAccountId, Guid destinationAccountId, decimal amount);
}
