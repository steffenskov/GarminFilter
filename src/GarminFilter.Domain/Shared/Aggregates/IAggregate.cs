namespace GarminFilter.Domain.Shared.Aggregates;

public interface IAggregate<out TId>
	where TId : IStrongTypedValue
{
	TId Id { get; }
}