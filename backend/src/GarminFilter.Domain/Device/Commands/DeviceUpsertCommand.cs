using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.SharedKernel.Device.Entities;

namespace GarminFilter.Domain.Device.Commands;

public record DeviceUpsertCommand(params IEnumerable<IGarminDevice> Devices) : IRequest;

file sealed class Handler : IRequestHandler<DeviceUpsertCommand>
{
	private readonly IDeviceRepository _repository;

	public Handler(IDeviceRepository repository)
	{
		_repository = repository;
	}

	public Task Handle(DeviceUpsertCommand command, CancellationToken cancellationToken)
	{
		var devices = command.Devices.Select(DeviceAggregate.FromGarmin);

		_repository.Upsert(devices);
		return Task.CompletedTask;
	}
}