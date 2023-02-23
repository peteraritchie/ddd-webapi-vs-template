using Domain.Abstractions;

namespace Infrastructure;

public class SmtpEmailSender : IEmailSender
{
	public Task SendEmailAsync(string to, string from, string subject, string body)
	{
		throw new NotImplementedException();
	}
}
