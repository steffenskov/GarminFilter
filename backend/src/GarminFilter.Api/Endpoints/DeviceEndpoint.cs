using GarminFilter.Api.Models;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.Device.Queries;
using GarminFilter.SharedKernel.App.ValueObjects;
using GarminFilter.SharedKernel.Device.ValueObjects;

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

		builder.MapPost("/{deviceId}/watchface",
			async (DeviceId deviceId, AppQueryModel model, IMediator mediator, CancellationToken cancellationToken) =>
				await GetAppsAsync(mediator, deviceId, AppTypes.WatchFace, model, cancellationToken));

		builder.MapPost("/{deviceId}/app",
			async (DeviceId deviceId, AppQueryModel model, IMediator mediator, CancellationToken cancellationToken) =>
				await GetAppsAsync(mediator, deviceId, AppTypes.DeviceApp, model, cancellationToken));
		builder.MapPost("/{deviceId}/widget",
			async (DeviceId deviceId, AppQueryModel model, IMediator mediator, CancellationToken cancellationToken) =>
				await GetAppsAsync(mediator, deviceId, AppTypes.Widget, model, cancellationToken));
		builder.MapPost("/{deviceId}/datafield",
			async (DeviceId deviceId, AppQueryModel model, IMediator mediator, CancellationToken cancellationToken) =>
				await GetAppsAsync(mediator, deviceId, AppTypes.DataField, model, cancellationToken));
		builder.MapPost("/{deviceId}/music",
			async (DeviceId deviceId, AppQueryModel model, IMediator mediator, CancellationToken cancellationToken) =>
				await GetAppsAsync(mediator, deviceId, AppTypes.Music, model, cancellationToken));
	}

	public string GroupName => "device";

	private static async Task<IEnumerable<AppViewModel>> GetAppsAsync(IMediator mediator, DeviceId deviceId, AppType appType, AppQueryModel model, CancellationToken cancellationToken)
	{
		var apps = await mediator.Send(new AppQuery(deviceId, appType, model.Paid, model.Developer, model.ExcludePermissions, model.PageIndex, model.PageSize, model.OrderBy),
			cancellationToken);

		return apps
			.Select(garminApp => new AppViewModel(garminApp));
	}

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