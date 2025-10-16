using GarminFilter.Domain.Garmin.Services;

namespace GarminFilter.Api.Services;

public class ApiScraperService : BackgroundService
{
	private readonly IList<ISynchronizerFacade> _synchronizerFacades;

	public ApiScraperService(IEnumerable<ISynchronizerFacade> synchronizerFacades)
	{
		_synchronizerFacades = synchronizerFacades.ToList();
	}

	protected async override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		foreach (var synchronizer in _synchronizerFacades)
		{
			await synchronizer.SynchronizeAsync(stoppingToken);
		}
	}
}