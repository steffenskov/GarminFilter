using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.SharedKernel.Device.Entities;

public interface IGarminDevice
{
	string Name { get; }
	string[] AdditionalNames { get; }
	string? UrlName { get; }
	DeviceId Id { get; }
}