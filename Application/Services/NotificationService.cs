using Domain.Abstractions;

namespace Application.Services;

public class NotificationService : INotificationService
{
	private readonly IEmailSender emailSender;

	public NotificationService(IEmailSender emailSender)
	{
		this.emailSender = emailSender;
	}

	public void SendNotification(string address, string message)
	{
		throw new NotImplementedException();
	}
}