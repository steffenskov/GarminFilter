using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Api.Models;

public record DeviceViewModel
{
	private readonly string _name = "";
	private readonly string? _urlName;

	private DeviceViewModel(DeviceAggregate deviceAggregate)
	{
		Id = deviceAggregate.Id;
		Name = deviceAggregate.Name;
		_urlName = deviceAggregate.UrlName;
	}

	public string Name
	{
		get => _name;
		private init => _name = StripCharacters(value).Capitalize();
	}

	public DeviceId Id { get; }

	private static string StripCharacters(string name)
	{
		return name
			.Replace("™", "")
			.Replace("®", "")
			.Replace("ē", "e");
	}

	public static IEnumerable<DeviceViewModel> Create(DeviceAggregate deviceAggregate)
	{
		yield return new DeviceViewModel(deviceAggregate);
		foreach (var additionalName in deviceAggregate.AdditionalNames)
		{
			yield return new DeviceViewModel(deviceAggregate) { Name = additionalName };
		}
	}

	public DeviceViewModel IncludeDistinctName()
	{
		if (string.IsNullOrWhiteSpace(_urlName))
		{
			return this;
		}

		return this with
		{
			Name = $"{Name} ({_urlName})"
		};
	}
}