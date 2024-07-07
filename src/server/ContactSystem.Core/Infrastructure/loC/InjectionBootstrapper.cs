namespace ContactSystem.Core.Infrastructure.loC;

using Injectors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InjectionBootstrapper
{
	public static IServiceCollection InjectLayersDependency ( this IServiceCollection serviceCollection , IConfiguration configuration )
	{
		NotNull ( serviceCollection );
		NotNull ( configuration );

		serviceCollection.AddHttpContextAccessor ();

		EfInjector.Inject ( serviceCollection );
		ErrorHandlerInjector.Inject ( serviceCollection );
		MapperInjector.Inject ( serviceCollection );
		SessionInjector.Inject ( serviceCollection );

		return serviceCollection;
	}
}