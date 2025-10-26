using GarminFilter.SharedKernel.Device.Entities;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Domain.Device.Aggregates;

public record DeviceAggregate : IAggregate<DeviceId>
{
	public string Name { get; private init; } = default!;
	public string[] AdditionalNames { get; private init; } = [];
	public string? UrlName { get; private init; }
	public DeviceId Id { get; private init; } = default!;

	public static DeviceAggregate FromGarmin(IGarminDevice garminDevice)
	{
		return new DeviceAggregate
		{
			Id = garminDevice.Id,
			Name = garminDevice.Name,
			UrlName = garminDevice.UrlName,
			AdditionalNames = garminDevice.AdditionalNames
		};
	}
}