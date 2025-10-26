using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;
using LiteDB;

namespace GarminFilter.Infrastructure.Garmin.Repositories;

internal class SyncStateRepository : BaseAggregateRepository<SyncState, AppType>, ISyncStateRepository
{
	public SyncStateRepository(LiteDatabase db) : base(db, "sync_states")
	{
	}

	public override SyncState? GetSingle(AppType id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}
}