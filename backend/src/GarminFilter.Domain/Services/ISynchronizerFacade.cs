namespace GarminFilter.Domain.Services;

public interface ISynchronizerFacade
{
	Task SynchronizeAsync(CancellationToken cancellationToken = default);
}