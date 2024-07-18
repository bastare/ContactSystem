namespace ContactSystem.Core.Domain.Specifications;

using DynamicLinqDecorator.Common.Extensions;
using Core.Queries.Interfaces;
using Interfaces;
using Pagination.Common.Extensions;

public sealed record DynamicQuerySpecification : QuerySpecification
{
	public DynamicQuerySpecification ( IDynamicQuery dynamicQuery, bool withPagination = default )
	{
		ArgumentNullException.ThrowIfNull ( dynamicQuery );

		QueryInjector = query =>
		{
			if ( HasExpression ( dynamicQuery ) )
				query = query.Where ( dynamicQuery );

			if ( HasOrdering ( dynamicQuery ) )
				query = query.OrderBy ( dynamicQuery );

			if ( HasProjection ( dynamicQuery ) )
				query = query.Select ( dynamicQuery );

			if ( withPagination && HasPagination ( dynamicQuery ) )
				query = query.GetPagedRecords (
					dynamicQuery.Offset!.Value ,
					dynamicQuery.Limit!.Value );

			return query;

			static bool HasExpression ( IExpressionQuery? expressionQuery )
				=> !string.IsNullOrEmpty ( expressionQuery?.Expression );

			static bool HasOrdering ( IOrderQuery? orderQuery )
				=> !string.IsNullOrEmpty ( orderQuery?.OrderBy )
					&& orderQuery?.IsDescending is not null;

			static bool HasProjection ( IProjectionQuery? projectionQuery )
				=> !string.IsNullOrEmpty ( projectionQuery?.Projection );

			static bool HasPagination ( IPaginationQuery? paginationQuery )
				=> paginationQuery is { Limit: not null, Offset: not null };
		};
	}
}