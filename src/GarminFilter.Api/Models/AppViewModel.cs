using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Garmin.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppViewModel
{
	public AppViewModel(GarminApp app)
	{
		Id = app.Id;
		Name = app.AppLocalizations.FirstOrDefault()?.Name ?? "Unknown name";
		Type = app.TypeId;
	}

	public AppType Type { get; set; }

	public AppId Id { get; }
	public string Name { get; }
}