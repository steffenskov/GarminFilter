namespace GarminFilter.Domain.Shared.Repositories;

public interface IRepositoryWithExists<in TId>
	where TId : IStrongTypedId
{
	bool Exists(TId id);
}