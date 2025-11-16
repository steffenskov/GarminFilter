using GarminFilter.Client.Services;
using GarminFilter.Domain.Policies;
using GarminFilter.SharedKernel.App.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class DataFieldSynchronizerFacade : BaseAppSynchronizerFacade<DataFieldSynchronizerFacade>
{
	public DataFieldSynchronizerFacade(IGarminClient client, IMediator mediator, IDelayPolicy delayPolicy, ILogger<DataFieldSynchronizerFacade>? logger = null) : base(client, mediator,
		AppTypes.DataField, delayPolicy, logger)
	{
	}
}