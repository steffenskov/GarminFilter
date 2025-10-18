using GarminFilter.Infrastructure;
using GarminFilter.Infrastructure.Garmin.Policies;

namespace GarminFilter.IntegrationTests.Configuration;

public class ContainerFixture
{
	public ContainerFixture()
	{
		var services = new ServiceCollection();
		services.AddDomain(":memory:", new NoDelayPolicy());
		Provider = services.BuildServiceProvider();
	}

	public ServiceProvider Provider { get; }
}