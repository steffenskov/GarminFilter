using GarminFilter.Api.Models;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Endpoints;

public class OrderEndpoint : IEndpoint
{
	public string GroupName => "ordering";

	public void MapEndpoint(RouteGroupBuilder builder)
	{
		builder.MapGet("/", () =>
		{
			var values = Enum.GetValues<AppOrders>();

			return values
				.Select(value => new AppOrderViewModel(value))
				.OrderBy(order => order.Name);
		});
	}
}