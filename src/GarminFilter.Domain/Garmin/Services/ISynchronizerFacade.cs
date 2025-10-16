namespace GarminFilter.Domain.Garmin.Services;

public interface ISynchronizerFacade
{
	Task SynchronizeAsync(CancellationToken cancellationToken = default);
}