using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppViewModel
{
	public AppViewModel(IGarminApp app)
	{
		Id = app.Id;
		Name = app.Name;
		Type = app.TypeId;
		ImageUrl = $"/app/{app.IconFileId}";
		Url = $"https://apps.garmin.com/apps/{app.Id}";
		IsPaid = app.IsPaid;
		Permissions = app.Permissions
			.Select(permission => new AppPermissionViewModel(permission))
			.OrderBy(permission => permission.Description)
			.ToList();
		Developer = app.DeveloperName;
	}

	public string Developer { get; }


	public List<AppPermissionViewModel> Permissions { get; }

	public bool IsPaid { get; }

	public string Url { get; }

	public string ImageUrl { get; }

	public AppType Type { get; }

	public AppId Id { get; }
	public string Name { get; }
}