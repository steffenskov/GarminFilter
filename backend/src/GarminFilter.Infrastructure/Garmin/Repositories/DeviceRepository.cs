using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.SharedKernel.Device.ValueObjects;
using LiteDB;

namespace GarminFilter.Infrastructure.Garmin.Repositories;

internal class DeviceRepository : BaseAggregateRepository<DeviceAggregate, DeviceId>, IDeviceRepository
{
	public DeviceRepository(LiteDatabase db) : base(db, "devices")
	{
	}

	public override DeviceAggregate? GetSingle(DeviceId id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}
}