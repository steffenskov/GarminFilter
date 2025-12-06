using GarminFilter.Client.Services;
using GarminFilter.Domain.Policies;
using GarminFilter.SharedKernel.App.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class WatchFaceSynchronizerFacade : BaseAppSynchronizerFacade<WatchFaceSynchronizerFacade>
{
	public WatchFaceSynchronizerFacade(IGarminClient client, IMediator mediator, IDelayPolicy delayPolicy, TimeProvider timeProvider, ILogger<WatchFaceSynchronizerFacade>? logger = null) : base(
		client, mediator,
		AppTypes.WatchFace, delayPolicy, timeProvider, logger)
	{
	}
}