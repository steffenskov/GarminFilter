using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.ValueObjects;
using GarminFilter.Domain.Shared.Repositories;

namespace GarminFilter.Domain.App.Repositories;

public interface IGarminAppRepository : IAggregateRepository<GarminApp, AppId>, IRepositoryWithExists<AppId>
{
	IEnumerable<GarminApp> Query(DeviceId deviceId, AppType type, bool includePaid, ISet<AppPermission> excludePermissions, int pageIndex, int pageSize);
	int GetCount(AppType type);
}