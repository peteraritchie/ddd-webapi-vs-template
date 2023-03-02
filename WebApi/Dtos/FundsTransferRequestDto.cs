using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

[DisplayName("FundsTransferRequest")]
public class FundsTransferRequestDto
{
	/// <summary>
	///     The source account ID
	/// </summary>
	/// <example>478131a6-52fe-4dd6-9bc8-f2e01b34be6c</example>
	[Required]
	public Guid SourceAccountId { get; set; }

	/// <summary>
	///     The target account ID
	/// </summary>
	/// <example>f5eb6500-4ab6-466c-b483-325d21e67344</example>
	[Required]
	public Guid DestinationAccountId { get; set; }

	/// <summary>
	///     The amount to transfer
	/// </summary>
	/// <example>200.00</example>
	[Required]
	[Range(
		0.01,
		3000.0)]
	public decimal Amount { get; set; }
}