namespace ContactSystem.Infrastructure.loC.Injectors.PersistenceServicesInjectors;

using Configurations.EntityFrameworkTriggers.AuditionTriggers;
using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Persistence.Uow;
using Persistence.Uow.Interfaces;

[InjectionOrder ( order: uint.MaxValue )]
public sealed class EfInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		serviceCollection.AddDbContext<EfContext> (
			optionsAction: ( dbContextOptionsBuilder ) =>
			{
				dbContextOptionsBuilder
					.UseLoggerFactory ( loggerFactory: ResolveLoggerFactory ( serviceCollection ) )

					// TODO: Fix me for proper indexing
					// .UseSqlite ( "DataSource=:memory:" )

					.UseInMemoryDatabase ( "_" )

					.UseTriggers ( triggerOptions =>
						triggerOptions.AddTrigger<OnAuditionTrigger> () );
			} );

		serviceCollection.TryAddScoped<IUnitOfWork<int> , EfUnitOfWork<EfContext , int>> ();
		serviceCollection.TryAddScoped<IEfUnitOfWork<EfContext , int> , EfUnitOfWork<EfContext , int>> ();
		serviceCollection.TryAddScoped<ITransaction , EfUnitOfWork<EfContext , int>> ();

		static ILoggerFactory ResolveLoggerFactory ( IServiceCollection serviceCollection )
			=> serviceCollection.BuildServiceProvider ()
				.GetRequiredService<ILoggerFactory> ();
	}
}