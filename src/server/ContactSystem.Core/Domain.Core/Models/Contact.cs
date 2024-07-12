namespace ContactSystem.Core.Domain.Core.Models;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;
using Validators;

[Index ( nameof ( Email ) , IsUnique = true )]
public sealed class Contact :
	IAuditableModel<long>,
	IHasValidation
{
	public long Id { get; set; }

	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	// ? Create Regex Constraint in db
	public string? Email { get; set; }

	// ? Create Regex Constraint in db
	public string? Phone { get; set; }

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

	public bool IsValid ()
		=> Validate ()
			.IsValid;

	public ValidationResult Validate ()
		=> new CustomerValidator ()
			.Validate ( instance: this );

	public void ValidateAndThrow ()
	{
		new CustomerValidator ()
			.ValidateAndThrow ( instance: this );
	}
}