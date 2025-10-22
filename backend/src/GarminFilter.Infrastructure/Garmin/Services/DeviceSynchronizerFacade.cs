using GarminFilter.Domain.Device.Commands;
using GarminFilter.Domain.Services;

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
		var devices = await _client.GetDevicesAsync(cancellationToken);
		if (devices.Count == 0)
		{
			return;
		}

		_logger?.LogInformation("Upserting {deviceCount} devices", devices.Count);


		await _mediator.Send(new DeviceUpsertCommand(devices), cancellationToken);
	}
}