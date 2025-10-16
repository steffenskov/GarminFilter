using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Garmin.Repositories;

namespace GarminFilter.Domain.Garmin.Queries.Device;

public record DeviceGetAllQuery : IRequest<IEnumerable<GarminDevice>>;

file sealed class Handler : IRequestHandler<DeviceGetAllQuery, IEnumerable<GarminDevice>>
{
	private readonly IGarminDeviceRepository _repository;

	public Handler(IGarminDeviceRepository repository)
	{
		_repository = repository;
	}

	public Task<IEnumerable<GarminDevice>> Handle(DeviceGetAllQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_repository.GetAll());
	}
}