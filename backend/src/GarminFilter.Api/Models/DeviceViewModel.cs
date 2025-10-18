using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Api.Models;

public class DeviceViewModel
{
	public DeviceViewModel(GarminDevice device)
	{
		Id = device.Id;
		Name = device.Name.Capitalize();
	}

	public string Name { get; }
	public DeviceId Id { get; }
}