namespace ContactSystem.Domain.Core.Models.Contact;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[Index ( nameof ( Email ) , IsUnique = true )]
public sealed class Contact : IAuditableModel<int>
{
	public int Id { get; set; }

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

	public int CreatedBy { get; set; }

	public DateTime Created { get; set; }

	public int? LastModifiedBy { get; set; }

	public DateTime? LastModified { get; set; }

	object IAuditable.CreatedBy { get; set; }

	object? IAuditable.LastModifiedBy { get; set; }

	object IModel.Id => Id;
}