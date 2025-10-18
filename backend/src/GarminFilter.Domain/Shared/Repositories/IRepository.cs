namespace GarminFilter.Domain.Shared.Repositories;

public interface IAggregateRepository<T, in TId>
	where T : IAggregate<TId>
	where TId : IStrongTypedValue
{
	IEnumerable<T> GetAll();
	void Upsert(T entity);
	void Upsert(params IEnumerable<T> entities);
	T? GetSingle(TId id);
}