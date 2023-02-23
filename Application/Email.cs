namespace Application;

public class Email
{
	public Email(string subject, string body, IEnumerable<string> directRecipients, IEnumerable<string> copiedRecipients, IEnumerable<string> blindCopiedRecipients)
	{
		Subject = subject;
		Body = body;
		DirectRecipients = directRecipients;
		CopiedRecipients = copiedRecipients;
		BlindCopiedRecipients = blindCopiedRecipients;
	}

	public string Subject { get; }
	public string Body { get; }
	public IEnumerable<string> DirectRecipients { get; }
	public IEnumerable<string> CopiedRecipients { get; }
	public IEnumerable<string> BlindCopiedRecipients { get; }
}
