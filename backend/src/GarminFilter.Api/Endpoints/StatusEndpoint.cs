using GarminFilter.Api.Models;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.Sync.Queries;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Endpoints;

public class StatusEndpoint : IEndpoint
{
	public string GroupName => "status";

	public void MapEndpoint(RouteGroupBuilder builder)
	{
		builder.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
		{
			List<SyncStatusViewModel> result = [];
			foreach (var appType in Enum.GetValues<AppTypes>())
			{
				var state = await mediator.Send(new SyncStateGetSingleQuery(appType), cancellationToken);
				var count = await mediator.Send(new AppGetCountQuery(appType), cancellationToken);

				result.Add(new SyncStatusViewModel(appType, state, count));
			}

			return result;
		});
	}
}