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

	public IEnumerable<GarminApp> Query(DeviceId deviceId, AppType type)
	{
		return _collection.Find(aggregate => aggregate.TypeId == type && aggregate.CompatibleDeviceTypeIds.Contains(deviceId));
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