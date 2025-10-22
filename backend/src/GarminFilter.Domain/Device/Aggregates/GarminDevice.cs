using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Domain.Device.Aggregates;

public record GarminDevice : IAggregate<DeviceId>
{
	public required string Name { get; init; }
	public string[] AdditionalNames { get; init; } = [];
	public string? UrlName { get; init; }
	public required DeviceId Id { get; init; }
}