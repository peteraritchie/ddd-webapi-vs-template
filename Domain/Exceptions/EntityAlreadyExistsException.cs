namespace Domain.Exceptions
{
	public class EntityAlreadyExistsException : Exception
	{
		public EntityAlreadyExistsException(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
