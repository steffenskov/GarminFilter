using GarminFilter.Domain.Garmin.Commands.App;
using GarminFilter.Domain.Garmin.Policies;
using GarminFilter.Domain.Garmin.Queries.App;
using GarminFilter.Infrastructure.Garmin.Policies;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal abstract class BaseAppSynchronizerFacade<TSelf> : ISynchronizerFacade
	where TSelf : BaseAppSynchronizerFacade<TSelf>
{
	private readonly AppType _appType;
	private readonly IGarminClient _client;
	private readonly IDelayPolicy _delayPolicy;
	private readonly ILogger<TSelf>? _logger;
	private readonly IMediator _mediator;

	protected BaseAppSynchronizerFacade(IGarminClient client, IMediator mediator, AppType appType, IDelayPolicy delayPolicy, ILogger<TSelf>? logger = null)
	{
		_client = client;
		_mediator = mediator;
		_appType = appType;
		_delayPolicy = delayPolicy;
		_logger = logger;
	}

	public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
	{
		var pageIndex = 0; // TODO: Figure out how to do a *whole* run before doing the "if exists, break" strategy
		do
		{
			var apps = await _client.GetAppsAsync(pageIndex, _appType, cancellationToken);
			if (apps.Count == 0)
			{
				break;
			}

			foreach (var app in apps)
			{
				var exists = await _mediator.Send(new AppExistsQuery(app.Id), cancellationToken);
				if (exists)
				{
					_logger?.LogInformation("App {id} already exists, stopping {appType} sync.", app.Id, _appType);
					return; // All caught up, no more sync needed
				}

				_logger?.LogInformation("Upserting App {id}: {app}", app.Id, app);
				await _mediator.Send(new AppUpsertCommand(app), cancellationToken);
			}

			await _delayPolicy.WaitForDelayAsync(cancellationToken); // Do not spam the API
		} while ((pageIndex += Consts.App.PageSize) < int.MaxValue);
	}
}