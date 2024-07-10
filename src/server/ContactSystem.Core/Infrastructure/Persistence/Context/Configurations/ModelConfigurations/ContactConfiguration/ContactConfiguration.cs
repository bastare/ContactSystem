namespace ContactSystem.Core.Infrastructure.Persistence.Context.Configurations.ModelConfigurations.ContactConfiguration;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ContactConfiguration : AuditableEntityTypeConfiguration<Contact , long>
{
	public override void Configure ( EntityTypeBuilder<Contact> builder )
	{
		builder.Property ( contact => contact.FirstName )
			.IsRequired ();

		builder.Property ( contact => contact.LastName )
			.IsRequired ();

		builder.Property ( contact => contact.Email )
			.IsRequired ();

		builder.Property ( contact => contact.Phone )
			.IsRequired ();

		builder.Property ( contact => contact.Title )
			.IsRequired ();

		base.Configure ( builder );
	}
}