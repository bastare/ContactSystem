namespace ContactSystem.Core.Infrastructure.Persistence.Context.Configurations.ModelConfigurations.ContactConfiguration;

using Domain.Contact;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ContactConfiguration : AuditableEntityTypeConfiguration<Contact , long>
{
	public override void Configure ( EntityTypeBuilder<Contact> builder )
	{
		base.Configure ( builder );
	}
}