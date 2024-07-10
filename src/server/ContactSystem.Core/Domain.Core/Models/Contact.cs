namespace ContactSystem.Core.Domain.Core.Models;

using Validators.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentValidation.Results;

[Index ( nameof ( Email ) , IsUnique = true )]
public sealed class Contact :
	IAuditableModel<long>,
	IHasValidationAsync
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

	public async Task<bool> IsValidAsync ( CancellationToken cancellationToken = default )
	{
		var validationResult =
			await new CustomerValidator ()
				.ValidateAsync ( instance: this , cancellationToken );

		return validationResult.IsValid;
	}

	public Task ValidateAndThrowAsync ( CancellationToken cancellationToken = default )
		=> new CustomerValidator ()
			.ValidateAndThrowAsync ( instance: this , cancellationToken );

	public Task<ValidationResult> ValidateAsync ( CancellationToken cancellationToken = default )
		=> new CustomerValidator ()
			.ValidateAsync ( instance: this , cancellationToken );
}