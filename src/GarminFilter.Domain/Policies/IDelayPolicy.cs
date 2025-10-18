namespace GarminFilter.Domain.Policies;

public interface IDelayPolicy
{
	ValueTask WaitForDelayAsync(CancellationToken cancellationToken = default);
}