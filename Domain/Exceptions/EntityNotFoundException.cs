namespace Domain.Exceptions;

public class EntityNotFoundException : Exception
{
	public EntityNotFoundException(Guid id)
	{
		Id = id;
	}

	public Guid Id { get; }
}
