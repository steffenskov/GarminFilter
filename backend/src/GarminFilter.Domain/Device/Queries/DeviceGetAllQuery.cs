using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Repositories;

namespace GarminFilter.Domain.Device.Queries;

public record DeviceGetAllQuery : IRequest<IEnumerable<DeviceAggregate>>;

file sealed class Handler : IRequestHandler<DeviceGetAllQuery, IEnumerable<DeviceAggregate>>
{
	private readonly IDeviceRepository _repository;

	public Handler(IDeviceRepository repository)
	{
		_repository = repository;
	}

	public Task<IEnumerable<DeviceAggregate>> Handle(DeviceGetAllQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_repository.GetAll());
	}
}