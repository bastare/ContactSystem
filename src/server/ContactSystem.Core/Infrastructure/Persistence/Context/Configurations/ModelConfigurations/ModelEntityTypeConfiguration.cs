namespace ContactSystem.Core.Infrastructure.Persistence.Context.Configurations.ModelConfigurations;

using ContactSystem.Core.Domain.Core;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class ModelEntityTypeConfiguration<TModelEntity, TKey> : IEntityTypeConfiguration<TModelEntity>
	where TModelEntity : class, IModel<TKey>
	where TKey : struct
{
	public virtual void Configure ( EntityTypeBuilder<TModelEntity> builder )
	{
		ConfigurePrimaryKey ();

		void ConfigurePrimaryKey ()
		{
			builder.Property ( model => model.Id )
				.IsRequired ();
		}
	}
}