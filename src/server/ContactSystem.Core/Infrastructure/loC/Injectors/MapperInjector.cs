namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class MapperInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		BootstrapTypeAdapterConfig ( typeAdapterConfig =>
		{
			serviceCollection.TryAddSingleton ( typeAdapterConfig );

			serviceCollection.TryAddScoped<IMapper , ServiceMapper> ();
		} );

		static void BootstrapTypeAdapterConfig ( Action<TypeAdapterConfig> injectTypeAdapterConfig )
		{
			TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;
			TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;

			TypeAdapterConfig.GlobalSettings.Default.AvoidInlineMapping ( value: true );

			TypeAdapterConfig.GlobalSettings.Scan ( Assembly.GetEntryAssembly ()! );

			injectTypeAdapterConfig ( TypeAdapterConfig.GlobalSettings );
		}
	}
}