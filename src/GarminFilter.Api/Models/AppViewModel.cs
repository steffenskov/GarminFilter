using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.ValueObjects;

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