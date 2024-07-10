namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Interfaces;
using Mapster;
using MapsterMapper;

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