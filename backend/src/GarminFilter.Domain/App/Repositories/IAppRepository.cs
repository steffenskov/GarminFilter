using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.Shared.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Domain.App.Repositories;

public interface IAppRepository : IAggregateRepository<AppAggregate, AppId>, IRepositoryWithExists<AppId>
{
	IEnumerable<AppAggregate> Query(DeviceId deviceId, AppType type, bool includePaid, ISet<AppPermission> excludePermissions, int pageIndex, int pageSize, AppOrder orderBy);
	int GetCount(AppType type);
}