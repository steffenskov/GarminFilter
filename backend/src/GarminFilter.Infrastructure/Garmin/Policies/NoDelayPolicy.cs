using GarminFilter.Domain.Policies;

namespace GarminFilter.Infrastructure.Garmin.Policies;

internal class NoDelayPolicy : IDelayPolicy
{
	public ValueTask WaitForDelayAsync(CancellationToken cancellationToken = default)
	{
		return ValueTask.CompletedTask;
	}
}