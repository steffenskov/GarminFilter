using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Repositories;

namespace GarminFilter.Domain.Device.Commands;

public record DeviceUpsertCommand(params IEnumerable<GarminDevice> Devices) : IRequest;

file sealed class Handler : IRequestHandler<DeviceUpsertCommand>
{
	private readonly IGarminDeviceRepository _repository;

	public Handler(IGarminDeviceRepository repository)
	{
		_repository = repository;
	}

	public Task Handle(DeviceUpsertCommand request, CancellationToken cancellationToken)
	{
		_repository.Upsert(request.Devices);
		return Task.CompletedTask;
	}
}