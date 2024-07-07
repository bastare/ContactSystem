namespace ContactSystem.Core.Domain.Contact;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[Index ( nameof ( Email ) , IsUnique = true )]
public sealed class Contact : IAuditableModel<long>
{
	public long Id { get; set; }

	[Required]
	public string? FirstName { get; set; }

	[Required]
	public string? LastName { get; set; }

	[Required]
	[EmailAddress]
	public string? Email { get; set; }

	[Required]
	[Phone]
	public string? Phone { get; set; }

	[Required]
	public string? Title { get; set; }

	public string? MiddleInitial { get; set; }

	public long CreatedBy { get; set; }

	public DateTime Created { get; set; }

	public long? LastModifiedBy { get; set; }

	public DateTime? LastModified { get; set; }

	object IAuditable.CreatedBy
	{
		get => CreatedBy;
		set => CreatedBy = ( long ) value;
	}

	object? IAuditable.LastModifiedBy
	{
		get => LastModifiedBy;
		set => LastModifiedBy = ( long ) value!;
	}

	object IModel.Id
	{
		get => Id;
		set => Id = ( long ) value;
	}
}