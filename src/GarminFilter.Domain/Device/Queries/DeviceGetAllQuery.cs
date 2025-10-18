using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Repositories;

namespace GarminFilter.Domain.Device.Queries;

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