namespace GarminFilter.Domain.Shared.Aggregates;

public interface IAggregate<out TId>
	where TId : IStrongTypedId
{
	TId Id { get; }
}