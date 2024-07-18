namespace ContactSystem.Core.Infrastructure.CrossCutting;

using loC.Injectors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InjectionBootstrapper
{
	public static IServiceCollection InjectLayersDependency ( this IServiceCollection serviceCollection , IConfiguration configuration )
	{
		ArgumentNullException.ThrowIfNull ( serviceCollection );
		ArgumentNullException.ThrowIfNull ( configuration );

		serviceCollection.AddHttpContextAccessor ();

		EfInjector.Inject ( serviceCollection );
		ErrorHandlerInjector.Inject ( serviceCollection );
		MapperInjector.Inject ( serviceCollection );
		SessionInjector.Inject ( serviceCollection );
		FluentValidationInjector.Inject ( serviceCollection );

		return serviceCollection;
	}
}