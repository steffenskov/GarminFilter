using GarminFilter.Domain.Shared.Repositories;
using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Domain.Sync.Repositories;

public interface ISyncStateRepository : IAggregateRepository<SyncState, AppType>
{
}