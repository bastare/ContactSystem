namespace ContactSystem.Core.Api.Endpoints.v1.Contracts;

using Domain.Core.Queries.Interfaces;
using Domain.Core;
using Domain.Core.Queries.Validators;
using FluentValidation;
using FluentValidation.Results;

public sealed record GetContactsQuery :
	IDynamicQuery,
	IHasValidation
{
	public int? Offset { get; init; }

	public int? Limit { get; init; }

	public bool? IsDescending { get; init; }

	public string? OrderBy { get; init; }

	public string? Projection { get; init; }

	public string? Expression { get; init; }

	public bool IsValid ()
		=> Validate ()
			.IsValid;

	public ValidationResult Validate ()
		=> new PaginationQueryValidator ()
			.Validate ( instance: this );

	public void ValidateAndThrow ()
	{
		new PaginationQueryValidator ()
			.ValidateAndThrow ( instance: this );
	}
}