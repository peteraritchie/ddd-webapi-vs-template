using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public interface IBuilder<out TResult>
{
	TResult Build();
	void Validate();
	bool TryValidate(out ValidationResult? result);
}