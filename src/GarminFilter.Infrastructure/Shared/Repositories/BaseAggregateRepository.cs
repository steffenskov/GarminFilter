using GarminFilter.Domain.Shared.Aggregates;
using GarminFilter.Domain.Shared.Repositories;
using LiteDB;
using StrongTypedId;

namespace GarminFilter.Infrastructure.Shared.Repositories;

internal abstract class BaseAggregateRepository<T, TId> : IAggregateRepository<T, TId>
	where T : IAggregate<TId>
	where TId : IStrongTypedId
{
	private readonly ILiteCollection<T> _collection;

	protected BaseAggregateRepository(LiteDatabase db, string collectionName)
	{
		_collection = db.GetCollection<T>(collectionName);
	}

	public IEnumerable<T> GetAll()
	{
		return _collection.FindAll();
	}

	public void Upsert(T entity)
	{
		_collection.Upsert(entity);
	}

	public T? GetSingle(TId id)
	{
		return _collection.FindOne(e => e.Id.Equals(id));
	}
}