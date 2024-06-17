namespace ContactSystem.Infrastructure.Persistence.Context.Configurations.ModelConfigurations.ContactConfiguration;

using ContactSystem.Domain.Core.Models.Contact;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ContactConfiguration : AuditableEntityTypeConfiguration<Contact , int>
{
	public override void Configure ( EntityTypeBuilder<Contact> builder ) {
		builder.Property ( contact => contact.FirstName )
			.IsRequired ();

		builder.Property ( contact => contact.LastName )
			.IsRequired ();

		builder.Property ( contact => contact.Email )
			.IsRequired ();

		builder.HasIndex ( contact => contact.Email )
			.IsUnique ();

		builder.Property ( contact => contact.Phone )
			.IsRequired ();

		builder.Property ( contact => contact.Title )
			.IsRequired ();

		base.Configure ( builder );
	}
}