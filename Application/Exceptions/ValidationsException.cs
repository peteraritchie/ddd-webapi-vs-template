using System.ComponentModel.DataAnnotations;

namespace Application.Exceptions;

public class ValidationsException : Exception
{
	private readonly List<ValidationResult> results;

	public ValidationsException(IEnumerable<ValidationResult> results)
	{
		this.results = new List<ValidationResult>(results);
	}

	public ValidationsException(ValidationResult result)
	{
		results = new List<ValidationResult> { result };
	}

	public void Add(ValidationResult result)
	{
		results.Add(result);
	}

	public IDictionary<string, string> ToDictionary()
	{
		var x = results.SelectMany(
			r => r.MemberNames.Select(
				m => new { MemberName = m, ErrorMessage = r.ErrorMessage! }));

		return x.ToDictionary(e=>e.MemberName, e=>e.ErrorMessage);
	}
}
