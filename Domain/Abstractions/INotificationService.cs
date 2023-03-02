namespace Domain.Abstractions;

public interface INotificationService
{
	void SendNotification(string address, string message);
}