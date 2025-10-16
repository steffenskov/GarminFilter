namespace GarminFilter.Domain.Shared.Repositories;

public interface IAggregateRepository<T, in TId>
	where T : IAggregate<TId>
	where TId : IStrongTypedId
{
	IEnumerable<T> GetAll();
	void Upsert(T entity);
	void Upsert(IEnumerable<T> entities);
	T? GetSingle(TId id);
}