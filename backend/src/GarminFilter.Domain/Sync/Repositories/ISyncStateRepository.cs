using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Shared.Repositories;
using GarminFilter.Domain.Sync.Aggregates;

namespace GarminFilter.Domain.Sync.Repositories;

public interface ISyncStateRepository : IAggregateRepository<SyncState, AppType>
{
}