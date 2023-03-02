using Domain.Exceptions;

namespace WebApi.Infrastructure;

public class InMemoryRepositoryBase<TEntity>
{
	protected readonly Dictionary<Guid, TEntity> entityDictionary = new();

	public Task CreateAsync(Guid entityId, TEntity entity)
	{
		if (entityDictionary.ContainsKey(entityId))
		{
			throw new EntityAlreadyExistsException(entityId);
		}

		entityDictionary[entityId] = entity;
		return Task.CompletedTask;
	}

	public Task DeleteAsync(Guid entityId)
	{
		if (!entityDictionary.ContainsKey(entityId))
		{
			throw new EntityNotFoundException(entityId);
		}

		entityDictionary.Remove(entityId);
		return Task.CompletedTask;
	}

	public Task<TEntity> UpdateAsync(Guid entityId, TEntity entity)
	{
		if (!entityDictionary.ContainsKey(entityId))
		{
			throw new EntityNotFoundException(entityId);
		}

		entityDictionary[entityId] = entity;
		// TODO: merge entities?
		return Task.FromResult(entity);
	}

	public Task<TEntity> GetAsync(Guid entityId)
	{
		if (!entityDictionary.ContainsKey(entityId))
		{
			throw new EntityNotFoundException(entityId);
		}

		return Task.FromResult(entityDictionary[entityId]);
	}
}