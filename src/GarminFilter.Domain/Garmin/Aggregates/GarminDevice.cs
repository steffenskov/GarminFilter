using GarminFilter.Domain.Shared.Aggregates;

namespace GarminFilter.Domain.Garmin.Aggregates;

public record GarminDevice : IAggregate<DeviceId>
{
	public required string Name { get; init; }
	public required DeviceId Id { get; init; }
}