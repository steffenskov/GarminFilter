using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Api.Models;

public record DeviceViewModel
{
	private readonly string? _urlName;

	private DeviceViewModel(GarminDevice device)
	{
		Id = device.Id;
		Name = StripCharacters(device.Name.Capitalize());
		_urlName = device.UrlName;
	}

	public string Name { get; private init; }
	public DeviceId Id { get; }

	private static string StripCharacters(string name)
	{
		return name
			.Replace("™", "")
			.Replace("®", "")
			.Replace("ē", "e");
	}

	public static IEnumerable<DeviceViewModel> Create(GarminDevice device)
	{
		yield return new DeviceViewModel(device);
		foreach (var additionalName in device.AdditionalNames)
		{
			yield return new DeviceViewModel(device with
			{
				Name = additionalName
			});
		}
	}

	public DeviceViewModel IncludeDistinctName()
	{
		return this with
		{
			Name = $"{Name} ({_urlName})"
		};
	}
}