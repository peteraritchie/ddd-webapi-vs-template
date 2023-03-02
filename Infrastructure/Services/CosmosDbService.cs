using System.Diagnostics.CodeAnalysis;
using Infrastructure.Common;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Services;

public class CosmosDbService<TEntity> where TEntity : IIdentifiable
{
	private readonly Container container;

	public CosmosDbService(
		CosmosClient cosmosClient,
		string databaseName,
		string containerName,
		string partitionKeyPath = "/id",
		int throughput = 400)
	{
		ArgumentNullException.ThrowIfNull(cosmosClient);

		ArgumentException.ThrowIfNullOrEmpty(databaseName);

		ArgumentException.ThrowIfNullOrEmpty(containerName);

		ArgumentException.ThrowIfNullOrEmpty(partitionKeyPath);

		Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName).Result;
		container = database.CreateContainerIfNotExistsAsync(
			containerName,
			partitionKeyPath,
			throughput).Result;
	}

	public async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(id);

		var document = await container.ReadItemAsync<TEntity>(
			id,
			new PartitionKey(id),
			cancellationToken: cancellationToken);
		return document.Resource;
	}

	public async Task<TEntity> CreateAsync([DisallowNull] TEntity entity,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entity);

		var response = await container
			.CreateItemAsync(
				entity,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
		return response.Resource;
	}

	public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(id);

		await container.DeleteItemAsync<TEntity>(
				id,
				new PartitionKey(id),
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entity);

		var response = await container
			.UpsertItemAsync(
				entity,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
		return response.Resource;
	}

	public async Task<TEntity> PatchItemAsync(string id, PatchOperation operation,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(id);

		var response = await container.PatchItemAsync<TEntity>(
				id,
				new PartitionKey(id),
				new[] { operation },
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return response.Resource;
	}

	public async Task<TEntity> PatchItemAsync(string id, IReadOnlyList<PatchOperation> operations,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(id);

		var response = await container.PatchItemAsync<TEntity>(
				id,
				new PartitionKey(id),
				operations,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return response.Resource;
	}
}
