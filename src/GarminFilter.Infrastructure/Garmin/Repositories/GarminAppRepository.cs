using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.ValueObjects;
using LiteDB;

namespace GarminFilter.Infrastructure.Garmin.Repositories;

internal class GarminAppRepository : BaseAggregateRepository<GarminApp, AppId>, IGarminAppRepository
{
	public GarminAppRepository(LiteDatabase db) : base(db, "apps")
	{
		_collection.EnsureIndex(app => app.TypeId);
	}

	public bool Exists(AppId id)
	{
		return _collection.Exists(aggregate => aggregate.Id == id);
	}

	public IEnumerable<GarminApp> Query(DeviceId deviceId, AppType type, ISet<AppPermission> excludePermissions)
	{
		return _collection.Find(aggregate =>
			aggregate.TypeId == type && aggregate.CompatibleDeviceTypeIds.Contains(deviceId) && !excludePermissions.Any(permission => aggregate.Permissions.Contains(permission)));
	}

	public IEnumerable<GarminApp> GetByType(AppType type)
	{
		return _collection.Find(aggregate => aggregate.TypeId == type.PrimitiveValue);
	}

	public override GarminApp? GetSingle(AppId id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}
}