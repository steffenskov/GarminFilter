using GarminFilter.Api.Models;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.Queries;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Api.Endpoints;

public class DeviceEndpoint : IEndpoint
{
	public void MapEndpoint(RouteGroupBuilder builder)
	{
		builder
			.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken) =>
			{
				var devices = await mediator.Send(new DeviceGetAllQuery(), cancellationToken);
				return devices;
			});

		builder.MapGet("/{deviceId}/watchface", async (DeviceId deviceId, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var watchfaces = await mediator.Send(new AppQuery(deviceId, AppTypes.WatchFace), cancellationToken);

			return watchfaces.Select(garminApp => new AppViewModel(garminApp));
		});
	}

	public string GroupName => "device";
}