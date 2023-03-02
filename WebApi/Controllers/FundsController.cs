using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Dtos;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FundsController : ControllerBase
{
	private readonly IFundsTransferService fundsTransferService;

	public FundsController(IFundsTransferService fundsTransferService)
	{
		this.fundsTransferService = fundsTransferService;
	}

	[HttpPost]
	[ProducesResponseType(
		typeof(ProblemDetails),
		StatusCodes.Status400BadRequest,
		ModernMediaTypeNames.Application.ProblemJson)]
	[ProducesResponseType(
		typeof(ProblemDetails),
		StatusCodes.Status404NotFound,
		ModernMediaTypeNames.Application.ProblemJson)]
	[ProducesResponseType(
		typeof(ProblemDetails),
		StatusCodes.Status500InternalServerError,
		ModernMediaTypeNames.Application.ProblemJson)]
	public IActionResult CreateFundTransferRequest(FundsTransferRequestDto fundsTransferRequest)
	{
		fundsTransferService.TransferAsync(
			fundsTransferRequest.SourceAccountId,
			fundsTransferRequest.DestinationAccountId,
			fundsTransferRequest.Amount);
		return NoContent();
	}
}