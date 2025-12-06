using GarminFilter.Client.Services;
using GarminFilter.Domain.Policies;
using GarminFilter.SharedKernel.App.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class WidgetSynchronizerFacade : BaseAppSynchronizerFacade<WidgetSynchronizerFacade>
{
	public WidgetSynchronizerFacade(IGarminClient client, IMediator mediator, IDelayPolicy delayPolicy, TimeProvider timeProvider, ILogger<WidgetSynchronizerFacade>? logger = null) : base(client,
		mediator,
		AppTypes.Widget, delayPolicy, timeProvider, logger)
	{
	}
}