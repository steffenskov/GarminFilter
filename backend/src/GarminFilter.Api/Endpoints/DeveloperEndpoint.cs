using GarminFilter.Domain.App.Queries;

namespace GarminFilter.Api.Endpoints;

public class DeveloperEndpoint : IEndpoint
{
	public string GroupName => "developer";

	public void MapEndpoint(RouteGroupBuilder builder)
	{
		builder.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
		{
			return await mediator.Send(new DeveloperGetAllQuery(), cancellationToken);
		});
	}
}