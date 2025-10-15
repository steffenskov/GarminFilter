using GarminFilter.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace GarminFilter.IntegrationTests.Configuration;

public class ContainerFixture
{
	public ContainerFixture()
	{
		var services = new ServiceCollection();
		services.AddDomain(":memory:");
		Provider = services.BuildServiceProvider();
	}

	public ServiceProvider Provider { get; }
}