using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Domain.App.Queries;

public record AppQuery(DeviceId DeviceId, AppType Type, bool? Paid, string? Developer, HashSet<AppPermission> ExcludePermissions, int PageIndex, int PageSize, AppOrder OrderBy)
	: IRequest<IEnumerable<AppAggregate>>;

file sealed class Handler : IRequestHandler<AppQuery, IEnumerable<AppAggregate>>
{
	private readonly IAppRepository _repository;

	public Handler(IAppRepository repository)
	{
		_repository = repository;
	}

	public Task<IEnumerable<AppAggregate>> Handle(AppQuery request, CancellationToken cancellationToken)
	{
		var result = _repository.Query(request.DeviceId, request.Type, request.Paid, request.Developer, request.ExcludePermissions, request.PageIndex, request.PageSize, request.OrderBy);

		return Task.FromResult(result);
	}
}