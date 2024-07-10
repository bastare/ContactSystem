namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Domain.Projections.Persistence;
using Domain.Projections.Persistence.Transactions;
using Interfaces;
using Persistence.Context;
using Persistence.Context.Configurations.Triggers;

public sealed class EfInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		var sqliteConnection_ = CreateAndPersistSqlConnection ( serviceCollection );

		serviceCollection.AddDbContext<EfContext> (
			optionsAction: ( dbContextOptionsBuilder ) =>
			{
				dbContextOptionsBuilder
					.UseLoggerFactory ( loggerFactory: ResolveLoggerFactory ( serviceCollection ) )

					.UseSqlite ( sqliteConnection_ )

					.UseTriggers ( triggerOptions =>
						triggerOptions.AddTrigger<OnAuditionTrigger> () );
			} );

		InjectProjections ( serviceCollection );
		EnsureCreated ( serviceCollection );

		static SqliteConnection CreateAndPersistSqlConnection ( IServiceCollection serviceCollection )
		{
			var sqliteConnection_ = new SqliteConnection ( "DataSource=:memory:" );
			sqliteConnection_.Open ();

			serviceCollection.TryAddSingleton ( sqliteConnection_ );

			return sqliteConnection_;
		}

		static ILoggerFactory ResolveLoggerFactory ( IServiceCollection serviceCollection )
			=> serviceCollection.BuildServiceProvider ()
				.GetRequiredService<ILoggerFactory> ();

		static void InjectProjections ( IServiceCollection serviceCollection )
		{
			serviceCollection.TryAddScoped<ITransaction , EfContext> ();
			serviceCollection.TryAddScoped<IContactSet , EfContext> ();
		}

		static void EnsureCreated ( IServiceCollection serviceCollection )
		{
			serviceCollection.BuildServiceProvider ()
				.GetRequiredService<EfContext> ()
					.Database.EnsureCreated ();
		}
	}
}