using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace WebApi.Common;

/// <summary>
///     A DTO type for communicating 201 Accepted response details as specified in
///     https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.2.3
/// </summary>
[ExcludeFromCodeCoverage(Justification = "In progress.")]
public class AcceptedResponse
{
	/// <summary>
	///     Initialize with status and status text
	/// </summary>
	/// <param name="statusText"></param>
	/// <param name="taskUri"></param>
	public AcceptedResponse(string statusText, Uri taskUri)
	{
		StatusText = statusText;
		TaskUri = taskUri;
	}

	/// <summary>
	///     Initialize with status and DateTime
	/// </summary>
	/// <param name="statusText"></param>
	/// <param name="retryAfterDate"></param>
	public AcceptedResponse(string statusText, DateTime retryAfterDate)
	{
		StatusText = statusText;
		RetryAfterDate = retryAfterDate;
	}

	/// <summary>
	///     Initialize with status and DateTimeOffset
	/// </summary>
	/// <param name="statusText"></param>
	/// <param name="retryAfterDate"></param>
	public AcceptedResponse(string statusText, DateTimeOffset retryAfterDate)
	{
		StatusText = statusText;
		RetryAfterDate = retryAfterDate;
	}

	/// <summary>
	///     The status of the accepted request, typically "pending" in a 202 Accepted content body
	/// </summary>
	[JsonPropertyName("status")]
	[Required]
	public string StatusText { get; set; }

	/// <summary>
	///     The URI to the endpoint where the status of the request can be checked.
	/// </summary>
	[JsonPropertyName("taskURI")]
	public Uri? TaskUri { get; set; }

	/// <summary>
	///     A ISO8601 formatted date/time estimate of when the task may be completed.
	/// </summary>
	[JsonPropertyName("retryAfter")]
	public DateTimeOffset? RetryAfterDate { get; set; }
}
