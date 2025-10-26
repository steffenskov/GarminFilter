using GarminFilter.Client.Services;
using GarminFilter.Domain.Device.Commands;
using GarminFilter.Domain.Services;
using Microsoft.Extensions.Logging;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class DeviceSynchronizerFacade : ISynchronizerFacade
{
	private readonly IGarminClient _client;
	private readonly ILogger<DeviceSynchronizerFacade>? _logger;
	private readonly IMediator _mediator;

	public DeviceSynchronizerFacade(IGarminClient client, IMediator mediator, ILogger<DeviceSynchronizerFacade>? logger = null)
	{
		_client = client;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
	{
		var garminDevices = await _client.GetDevicesAsync(cancellationToken);
		if (garminDevices.Count == 0)
		{
			return;
		}

		_logger?.LogInformation("Upserting {deviceCount} devices", garminDevices.Count);

		await _mediator.Send(new DeviceUpsertCommand(garminDevices), CancellationToken.None);
	}
}