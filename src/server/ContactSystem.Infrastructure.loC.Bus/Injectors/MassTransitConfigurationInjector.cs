namespace ContactSystem.Infrastructure.loC.Bus.Injectors;

using InjectorBuilder.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class MassTransitConfigurationInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.TryAddSingleton ( KebabCaseEndpointNameFormatter.Instance );
	}
}