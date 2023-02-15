namespace Core.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public Guid Id { get; }

        public EntityAlreadyExistsException(Guid id)
        {
            Id = id;
        }
    }
}
