using GarminFilter.Api.Models;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Endpoints;

public class PermissionEndpoint : IEndpoint
{
	public string GroupName => "permission";

	public void MapEndpoint(RouteGroupBuilder builder)
	{
		builder.MapGet("/", () =>
		{
			var values = Enum.GetValues<AppPermissions>();

			return values
				.Select(value => new AppPermissionViewModel(value))
				.OrderBy(permission => permission.Description);
		});
	}
}