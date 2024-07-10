namespace ContactSystem.Core.Api.Endpoints.v1.Contracts;

using Validators;
using Domain.Core;
using FluentValidation;
using FluentValidation.Results;

public sealed record ContactForPatchRequestBody : IHasValidation
{
	public string? FirstName { get; init; }

	public string? LastName { get; init; }

	public string? Email { get; init; }

	public string? Phone { get; init; }

	public string? Title { get; init; }

	public string? MiddleInitial { get; init; }

	public bool IsValid ()
		=> Validate ()
			.IsValid;

	public ValidationResult Validate ()
		=> new ContactForPatchRequestBodyValidator ()
			.Validate ( instance: this );

	public void ValidateAndThrow ()
	{
		new ContactForPatchRequestBodyValidator ()
			.ValidateAndThrow ( instance: this );
	}
}