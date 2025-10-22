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
				var result = devices
					.SelectMany(DeviceViewModel.Create)
					.ToList();

				return DistinguishDuplicateNames(result)
					.OrderBy(device => device.Name);
			});

		builder.MapPost("/{deviceId}/watchface", async (DeviceId deviceId, AppQueryModel model, IMediator mediator, CancellationToken cancellationToken) =>
		{
			var watchfaces = await mediator.Send(new AppQuery(deviceId, AppTypes.WatchFace, model.IncludePaid, model.ExcludePermissions, model.PageIndex, model.PageSize, model.OrderBy),
				cancellationToken);

			return watchfaces
				.Select(garminApp => new AppViewModel(garminApp));
		});
	}

	public string GroupName => "device";

	private static IEnumerable<DeviceViewModel> DistinguishDuplicateNames(List<DeviceViewModel> models)
	{
		foreach (var model in models)
		{
			if (models.Count(m => m.Name == model.Name) > 1)
			{
				yield return model.IncludeDistinctName();
			}
			else
			{
				yield return model;
			}
		}
	}
}