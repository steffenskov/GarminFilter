namespace GarminFilter.Domain.Garmin.Policies;

public interface IDelayPolicy
{
	ValueTask WaitForDelayAsync(CancellationToken cancellationToken = default);
}