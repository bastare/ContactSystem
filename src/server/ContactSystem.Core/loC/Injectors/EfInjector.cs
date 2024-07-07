namespace ContactSystem.Core.loC.Injectors;

using Configurations.EntityFrameworkTriggers.AuditionTriggers;
using ContactSystem.Core.Persistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

public static class EfInjector
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

		// TODO: Don't forget to create post-operation life-hook for injectors
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

		static void EnsureCreated ( IServiceCollection serviceCollection )
		{
			serviceCollection.BuildServiceProvider ()
				.GetRequiredService<EfContext> ()
					.Database.EnsureCreated ();
		}
	}
}