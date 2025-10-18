using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.Domain.Device.ValueObjects;
using LiteDB;

namespace GarminFilter.Infrastructure.Garmin.Repositories;

internal class GarminDeviceRepository : BaseAggregateRepository<GarminDevice, DeviceId>, IGarminDeviceRepository
{
	public GarminDeviceRepository(LiteDatabase db) : base(db, "devices")
	{
	}

	public override GarminDevice? GetSingle(DeviceId id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}
}