using System.Net;
using GarminFilter.Client.Services;
using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Commands;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Endpoints;

public class AppEndpoint : IEndpoint
{
	public string GroupName => "app";

	public void MapEndpoint(RouteGroupBuilder builder)
	{
		builder.MapGet("{appId}/{iconFileId}", async (AppId appId, Guid iconFileId, IGarminClient client, IMediator mediator, HttpContext http, CancellationToken cancellationToken) =>
		{
			try
			{
				return await GetIconAsync(client, iconFileId, http, cancellationToken);
			}
			catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			{
				var app = await UpdateAppAsync(appId, client, mediator, cancellationToken);
				return await GetIconAsync(client, app.IconFileId, http, cancellationToken);
			}
		});
	}

	private static async Task<AppAggregate> UpdateAppAsync(AppId appId, IGarminClient client, IMediator mediator, CancellationToken cancellationToken)
	{
		var updatedApp = await client.GetAppAsync(appId, cancellationToken);
		return await mediator.Send(new AppUpsertCommand(updatedApp), cancellationToken);
	}

	private static async Task<IResult> GetIconAsync(IGarminClient client, Guid iconFileId, HttpContext http, CancellationToken cancellationToken)
	{
		var stream = await client.GetFileAsync(iconFileId, cancellationToken);

		http.Response.Headers.CacheControl = $"public,max-age={TimeSpan.FromDays(90).TotalSeconds}";
		return Results.Stream(stream, "image/jpeg");
	}
}