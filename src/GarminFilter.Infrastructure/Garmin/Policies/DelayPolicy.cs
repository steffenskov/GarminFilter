using GarminFilter.Domain.Policies;

namespace GarminFilter.Infrastructure.Garmin.Policies;

public class DelayPolicy : IDelayPolicy
{
	private readonly TimeSpan _delay;

	public DelayPolicy(TimeSpan delay)
	{
		_delay = delay;
	}

	public async ValueTask WaitForDelayAsync(CancellationToken cancellationToken = default)
	{
		await Task.Delay(_delay, cancellationToken);
	}
}