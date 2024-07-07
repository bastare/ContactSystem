namespace ContactSystem.Core.Api.Queries.Validators;

using FluentValidation;

public sealed class PaginationQueryValidator : AbstractValidator<PaginationQuery>
{
	public PaginationQueryValidator ()
	{
		RuleFor ( paginationQuery => paginationQuery.Limit )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );

		RuleFor ( paginationQuery => paginationQuery.Offset )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );
	}
}