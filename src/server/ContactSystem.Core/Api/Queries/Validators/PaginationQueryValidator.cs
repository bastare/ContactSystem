namespace ContactSystem.Core.Api.Queries.Validators;

using Interfaces;
using FluentValidation;

public sealed class PaginationQueryValidator : AbstractValidator<IPaginationQuery>
{
	public PaginationQueryValidator ()
	{
		RuleFor ( paginationQuery => paginationQuery.Limit )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );

		RuleFor ( paginationQuery => paginationQuery.Offset )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );
	}
}