using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Shared.Repositories;

namespace GarminFilter.Domain.Garmin.Repositories;

public interface IGarminAppRepository : IAggregateRepository<GarminApp, AppId>, IRepositoryWithExists<AppId>
{
	IEnumerable<GarminApp> Query(DeviceId deviceId, AppType type);
	IEnumerable<GarminApp> GetByType(AppType type);
}