using GarminFilter.Client;
using GarminFilter.Client.Services;
using GarminFilter.Domain.App.Commands;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.Policies;
using GarminFilter.Domain.Services;
using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.Domain.Sync.Queries;
using GarminFilter.SharedKernel;
using GarminFilter.SharedKernel.App.ValueObjects;
using Microsoft.Extensions.Logging;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal abstract class BaseAppSynchronizerFacade<TSelf> : ISynchronizerFacade
	where TSelf : BaseAppSynchronizerFacade<TSelf>
{
	private readonly AppType _appType;
	private readonly IGarminClient _client;
	private readonly IDelayPolicy _delayPolicy;
	private readonly ILogger<TSelf>? _logger;
	private readonly IMediator _mediator;
	private readonly TimeProvider _timeProvider;

	protected BaseAppSynchronizerFacade(IGarminClient client, IMediator mediator, AppType appType, IDelayPolicy delayPolicy, TimeProvider timeProvider, ILogger<TSelf>? logger = null)
	{
		_client = client;
		_mediator = mediator;
		_appType = appType;
		_delayPolicy = delayPolicy;
		_logger = logger;
		_timeProvider = timeProvider;
	}

	public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
	{
		var state = await _mediator.Send(new SyncStateGetSingleQuery(_appType), cancellationToken);

		if (state?.ShouldRenewFullSync(_timeProvider) == true)
		{
			state = await _mediator.Send(new SyncStateRenewCommand(_appType), cancellationToken);
		}

		if (state?.InitialSyncCompleted == true)
		{
			await SynchronizeLatestAsync(cancellationToken);
		}
		else
		{
			await SynchronizeInitialAsync(state, cancellationToken);
		}
	}

	private async Task SynchronizeLatestAsync(CancellationToken cancellationToken)
	{
		var pageIndex = 0;
		var hasMorePages = true;

		while (hasMorePages && !cancellationToken.IsCancellationRequested)
		{
			var apps = await _client.GetAppsAsync(pageIndex, _appType, cancellationToken);
			
			// If no apps returned, we've reached the end
			if (apps.Count == 0)
			{
				hasMorePages = false;
				continue;
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

			// If all apps on this page already exist, continue to next page
			// This allows us to check if there are more pages with new apps
			if (existsCount == apps.Count)
			{
				_logger?.LogInformation("All apps on page {pageIndex} already exist, checking next page.", pageIndex);
				pageIndex++;
			}
			else
			{
				// If we found new apps, continue to next page to check for more
				pageIndex++;
			}

			await _delayPolicy.WaitForDelayAsync(cancellationToken); // Do not spam the API
		}
	}

	private async Task SynchronizeInitialAsync(SyncState? state, CancellationToken cancellationToken)
	{
		var pageIndex = state?.PageIndex ?? 0;

		do
		{
			var apps = await _client.GetAppsAsync(pageIndex, _appType, cancellationToken);
			if (apps.Count == 0)
			{
				await _mediator.Send(new SyncStateInitialCompletedCommand(_appType, _timeProvider.GetUtcNowDate()), cancellationToken);
				return;
			}

			foreach (var app in apps)
			{
				_logger?.LogInformation("Upserting App {id}: {app}", app.Id, app);
				await _mediator.Send(new AppUpsertCommand(app), cancellationToken);
			}

			await _mediator.Send(new SyncStatePageMovedCommand(_appType, pageIndex), cancellationToken);

			await _delayPolicy.WaitForDelayAsync(cancellationToken); // Do not spam the API
		} while ((pageIndex += Consts.App.PageSize) < int.MaxValue);
	}
}
