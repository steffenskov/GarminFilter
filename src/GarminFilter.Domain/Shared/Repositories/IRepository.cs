using GarminFilter.Domain.Shared.Aggregates;

namespace GarminFilter.Domain.Shared.Repositories;

public interface IAggregateRepository<T, in TId>
	where T : IAggregate<TId>
	where TId : IStrongTypedId
{
	IEnumerable<T> GetAll();
	void Upsert(T entity);
	T? GetSingle(TId id);
}