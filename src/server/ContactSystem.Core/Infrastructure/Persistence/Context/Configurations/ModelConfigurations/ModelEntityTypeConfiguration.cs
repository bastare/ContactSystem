namespace ContactSystem.Core.Infrastructure.Persistence.Context.Configurations.ModelConfigurations;

using Domain.Core;
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