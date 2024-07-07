namespace ContactSystem.Core.loC;

using ContactSystem.Core.loC.Injectors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InjectionBootstrapper
{
	public static IServiceCollection InjectLayersDependency ( this IServiceCollection serviceCollection , IConfiguration configuration )
	{
		NotNull ( serviceCollection );
		NotNull ( configuration );

		EfInjector.Inject ( serviceCollection );
		ErrorHandlerInjector.Inject ( serviceCollection );
		MapperInjector.Inject ( serviceCollection );

		return serviceCollection;
	}
}