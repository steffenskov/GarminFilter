using GarminFilter.Client.Services;

namespace GarminFilter.Api.Endpoints;

public class AppEndpoint : IEndpoint
{
	public string GroupName => "app";

	public void MapEndpoint(RouteGroupBuilder builder)
	{
		builder.MapGet("{iconFileId}", async (string iconFileId, IGarminClient client, HttpContext http, CancellationToken cancellationToken) =>
		{
			var stream = await client.GetFileAsync(iconFileId, cancellationToken);

			http.Response.Headers.CacheControl = $"public,max-age={TimeSpan.FromDays(90).TotalSeconds}";
			return Results.Stream(stream, "image/jpeg");
		});
	}
}