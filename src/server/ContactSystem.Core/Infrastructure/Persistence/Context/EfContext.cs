namespace ContactSystem.Core.Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using AgileObjects.NetStandardPolyfills;
using Configurations.ModelConfigurations;
using Domain.Projections.Persistence;
using Domain.Core.Models;

public sealed class EfContext ( DbContextOptions<EfContext> options ) :
	DbContext ( options ),
	IContactSet
{
	public DbSet<Contact> Contacts { get; set; }

	protected override void OnModelCreating ( ModelBuilder modelBuilder )
	{
		ApplyConfigurationsFromAssembly ( modelBuilder );

		static void ApplyConfigurationsFromAssembly ( ModelBuilder modelBuilder )
		{
			modelBuilder.ApplyConfigurationsFromAssembly ( assembly: GetAssemblyWithConfigurations () );

			static Assembly GetAssemblyWithConfigurations ()
				=> typeof ( ModelEntityTypeConfiguration<,> )
					.GetAssembly ();
		}
	}
}