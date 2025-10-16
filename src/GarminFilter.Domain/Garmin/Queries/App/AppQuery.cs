using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Garmin.Repositories;

namespace GarminFilter.Domain.Garmin.Queries.App;

public record AppQuery(DeviceId DeviceId, AppType Type) : IRequest<IEnumerable<GarminApp>>;

file sealed class Handler : IRequestHandler<AppQuery, IEnumerable<GarminApp>>
{
	private readonly IGarminAppRepository _repository;

	public Handler(IGarminAppRepository repository)
	{
		_repository = repository;
	}

	public Task<IEnumerable<GarminApp>> Handle(AppQuery request, CancellationToken cancellationToken)
	{
		var result = _repository.Query(request.DeviceId, request.Type);

		return Task.FromResult(result);
	}
}