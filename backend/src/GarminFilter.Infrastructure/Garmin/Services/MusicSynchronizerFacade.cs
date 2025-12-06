using GarminFilter.Client.Services;
using GarminFilter.Domain.Policies;
using GarminFilter.SharedKernel.App.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class MusicSynchronizerFacade : BaseAppSynchronizerFacade<MusicSynchronizerFacade>
{
	public MusicSynchronizerFacade(IGarminClient client, IMediator mediator, IDelayPolicy delayPolicy, TimeProvider timeProvider, ILogger<MusicSynchronizerFacade>? logger = null) : base(client,
		mediator,
		AppTypes.Music, delayPolicy, timeProvider, logger)
	{
	}
}