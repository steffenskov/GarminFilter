using GarminFilter.Domain.App.Aggregates;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppViewModel
{
	public AppViewModel(AppAggregate app)
	{
		Id = app.Id;
		Name = app.Name;
		Type = app.Type;
		ImageUrl = $"/app/{app.IconFileId}";
		Url = $"https://apps.garmin.com/apps/{app.Id}";
		IsPaid = app.IsPaid;
		Permissions = app.RequiredPermissions
			.Select(permission => new AppPermissionViewModel(permission))
			.OrderBy(permission => permission.Description)
			.ToList();
		Developer = app.DeveloperName;
		AverageRating = app.AverageRating;
		ReviewCount = app.ReviewCount;
		ReleaseDate = DateTime.UnixEpoch.AddMilliseconds(app.ReleaseDate).ToString("MMM dd, yyyy");
	}

	public string ReleaseDate { get; }

	public uint ReviewCount { get; }

	public decimal AverageRating { get; }

	public string Developer { get; }


	public List<AppPermissionViewModel> Permissions { get; }

	public bool IsPaid { get; }

	public string Url { get; }

	public string ImageUrl { get; }

	public AppType Type { get; }

	public AppId Id { get; }
	public string Name { get; }
}