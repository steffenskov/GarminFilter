using System.Diagnostics;
using GarminFilter.Domain.App.Commands;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Policies;
using GarminFilter.Domain.Services;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.Domain.Sync.Queries;

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
		var state = await _mediator.Send(new SyncStateGetSingleQuery(_appType), cancellationToken);
		var pageIndex = 0;


		if (state is not null && !state.InitialSyncCompleted)
		{
			pageIndex = state.PageIndex ?? throw new UnreachableException("State has null PageIndex despite not being InitialSyncCompleted");
		}

		do
		{
			var apps = await _client.GetAppsAsync(pageIndex, _appType, cancellationToken);
			if (apps.Count == 0)
			{
				await _mediator.Send(new SyncStateInitialCompletedCommand(_appType), cancellationToken);
				return;
			}

			var existsCount = 0;
			foreach (var app in apps)
			{
				var exists = await _mediator.Send(new AppExistsQuery(app.Id), cancellationToken);
				if (exists)
				{
					existsCount++;
					continue;
				}

				_logger?.LogInformation("Upserting App {id}: {app}", app.Id, app);
				await _mediator.Send(new AppUpsertCommand(app), cancellationToken);
			}

			if (state?.InitialSyncCompleted == true && existsCount == apps.Count) // Found no new apps, break
			{
				_logger?.LogInformation("All apps fetched already exists, stopping {appType} sync.", _appType);
				return;
			}

			if (state?.InitialSyncCompleted != true)
			{
				await _mediator.Send(new SyncStatePageMovedCommand(_appType, pageIndex), cancellationToken);
			}

			await _delayPolicy.WaitForDelayAsync(cancellationToken); // Do not spam the API
		} while ((pageIndex += Consts.App.PageSize) < int.MaxValue);
	}
}