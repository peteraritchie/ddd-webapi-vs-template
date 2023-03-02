using System.ComponentModel.DataAnnotations;
using Application.Translators.Validators;
using Domain.Common;

namespace Application.Translators.Builders;

internal class EmailBuilder : IBuilder<Email>
{
	public List<string> Recipients { get; } = new();
	public List<string> CopiedRecipients { get; } = new();
	public List<string> BlindCopiedRecipients { get; } = new();
	public string? Subject { get; private set; }
	public string? Body { get; private set; }

	public Email Build()
	{
		Validate();
		return new Email(
			Subject!,
			Body!,
			Recipients,
			CopiedRecipients,
			BlindCopiedRecipients);
	}

	public void Validate()
	{
		if (!TryValidate(out var result))
		{
			throw new ValidationException(
				result!,
				default,
				default);
		}
	}

	public bool TryValidate(out ValidationResult? result)
	{
		if (!EmailParametersValidator.TryValidate(
			    Recipients,
			    CopiedRecipients,
			    BlindCopiedRecipients,
			    Subject,
			    Body,
			    out result))
		{
			throw new ValidationException(
				result!,
				default,
				default);
		}

		return true;
	}

	public EmailBuilder To(string email)
	{
		Recipients.Add(email);
		return this;
	}

	public EmailBuilder WithSubject(string subject)
	{
		Subject = subject;
		return this;
	}

	public EmailBuilder WithBody(string body)
	{
		Body = body;
		return this;
	}

	public EmailBuilder Copy(string email)
	{
		CopiedRecipients.Add(email);
		return this;
	}

	public EmailBuilder BlindCopy(string email)
	{
		BlindCopiedRecipients.Add(email);
		return this;
	}
}