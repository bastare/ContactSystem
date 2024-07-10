namespace ContactSystem.Core.Infrastructure.Persistence.Context.Configurations.ModelConfigurations;

using ContactSystem.Core.Domain.Core;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class AuditableEntityTypeConfiguration<TAuditableEntity, TKey> : ModelEntityTypeConfiguration<TAuditableEntity , TKey>
	where TAuditableEntity : class, IAuditableModel<TKey>
	where TKey : struct
{
	public override void Configure ( EntityTypeBuilder<TAuditableEntity> builder )
	{
		ConfigureAuditableFields ();

		base.Configure ( builder );

		void ConfigureAuditableFields ()
		{
			builder.Property ( auditableModel => auditableModel.Created )
				.IsRequired ();
		}
	}
}