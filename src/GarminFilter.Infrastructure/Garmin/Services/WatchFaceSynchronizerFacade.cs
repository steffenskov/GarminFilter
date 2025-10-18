using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Policies;
using GarminFilter.Domain.Services;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class WatchFaceSynchronizerFacade : BaseAppSynchronizerFacade<WatchFaceSynchronizerFacade>
{
	public WatchFaceSynchronizerFacade(IGarminClient client, IMediator mediator, IDelayPolicy delayPolicy, ILogger<WatchFaceSynchronizerFacade>? logger = null) : base(client, mediator,
		AppTypes.WatchFace, delayPolicy, logger)
	{
	}
}