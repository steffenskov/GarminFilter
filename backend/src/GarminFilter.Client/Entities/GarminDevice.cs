using GarminFilter.SharedKernel.Device.Entities;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Client.Entities;

public record GarminDevice : IGarminDevice
{
	public required string Name { get; init; }
	public string[] AdditionalNames { get; init; } = [];
	public string? UrlName { get; init; }
	public required DeviceId Id { get; init; }
}