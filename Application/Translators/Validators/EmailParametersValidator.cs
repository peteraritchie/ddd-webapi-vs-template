using System.ComponentModel.DataAnnotations;

namespace Application.Translators.Validators;

public static class EmailParametersValidator
{
	public static bool TryValidate(
		IEnumerable<string> recipients,
		IEnumerable<string> copiedRecipients,
		IEnumerable<string> blindCopiedRecipients,
		string? subject,
		string? body,
		out ValidationResult? result)
	{
		if (!recipients.Any())
		{
			_ = new ValidationResult(
				"At least one recipient is required",
				new[] { nameof(Email.DirectRecipients) });
		}

		if (subject == null)
		{
			_ = new ValidationResult(
				"A subject is required",
				new[] { nameof(Email.Subject) });
		}

		if (body == null)
		{
			_ = new ValidationResult(
				"A body is required",
				new[] { nameof(Email.Body) });
		}

		result = null;
		return true;
	}
}