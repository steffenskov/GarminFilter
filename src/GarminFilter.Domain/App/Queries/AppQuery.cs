using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Domain.App.Queries;

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