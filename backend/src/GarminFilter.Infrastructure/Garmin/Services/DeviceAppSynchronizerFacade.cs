using GarminFilter.Client.Services;
using GarminFilter.Domain.Policies;
using GarminFilter.SharedKernel.App.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class DeviceAppSynchronizerFacade : BaseAppSynchronizerFacade<DeviceAppSynchronizerFacade>
{
	public DeviceAppSynchronizerFacade(IGarminClient client, IMediator mediator, IDelayPolicy delayPolicy, TimeProvider timeProvider, ILogger<DeviceAppSynchronizerFacade>? logger = null) : base(
		client, mediator,
		AppTypes.DeviceApp, delayPolicy, timeProvider, logger)
	{
	}
}