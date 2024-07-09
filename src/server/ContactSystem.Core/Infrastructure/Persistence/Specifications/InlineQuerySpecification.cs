namespace ContactSystem.Core.Infrastructure.Persistence.Specifications;

using Api.Queries.Interfaces;
using DynamicLinqDecorator.Common.Extensions;
using Interfaces;

public sealed record InlineQuerySpecification : IQuerySpecification
{
	public Func<IQueryable , IQueryable>? QueryInjector { get; private init; }

	public InlineQuerySpecification (
		IExpressionQuery? expressionQuery = default ,
		IOrderQuery? orderQuery = default ,
		IProjectionQuery? projectionQuery = default )
	{
		QueryInjector = query =>
		{
			if ( HasExpression ( expressionQuery ) )
				query = query.Where ( expressionQuery );

			if ( HasOrdering ( orderQuery ) )
				query = query.OrderBy ( orderQuery );

			if ( HasProjection ( projectionQuery ) )
				query = query.Select ( projectionQuery );

			return query;

			static bool HasExpression ( IExpressionQuery? expressionQuery )
				=> !string.IsNullOrEmpty ( expressionQuery?.Expression );

			static bool HasOrdering ( IOrderQuery? orderQuery )
				=> !string.IsNullOrEmpty ( orderQuery?.OrderBy )
					&& orderQuery?.IsDescending is not null;

			static bool HasProjection ( IProjectionQuery? projectionQuery )
				=> !string.IsNullOrEmpty ( projectionQuery?.Projection );
		};
	}
}