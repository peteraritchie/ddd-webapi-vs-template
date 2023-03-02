namespace Domain.Exceptions
{
	public class EntityNotFoundException : Exception
	{
		public EntityNotFoundException(Guid id)
			: base($"Entity with ID {id} was not found.")
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
