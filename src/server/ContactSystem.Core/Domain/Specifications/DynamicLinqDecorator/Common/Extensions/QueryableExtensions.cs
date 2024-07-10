namespace ContactSystem.Core.Domain.Specifications.DynamicLinqDecorator.Common.Extensions;

using Core.Queries.Interfaces;
using System.Linq.Dynamic.Core;

public static class QueryableExtensions
{
	public static IQueryable Where ( this IQueryable query , IExpressionQuery? expressionQuery )
	{
		NotNull ( query );
		NotNullOrEmpty ( expressionQuery?.Expression );

		try
		{
			return query.Where ( expressionQuery!.Expression! );
		}
		catch ( Exception exception )
		{
			throw new LinqSyntaxException (
				message: string.Concat ( "Expression: " , exception.Message ) ,
				innerException: exception );
		}
	}

	public static IOrderedQueryable OrderBy ( this IQueryable query , IOrderQuery? orderQuery )
	{
		NotNull ( query );
		NotNullOrEmpty ( orderQuery?.OrderBy );

		try
		{
			return query.OrderBy (
				ordering: string.Join (
					separator: ' ' ,
					orderQuery!.OrderBy ,
					orderQuery.IsDescending.GetValueOrDefault ()
						? "desc"
						: "asc"
				)
			);
		}
		catch ( Exception exception )
		{
			throw new LinqSyntaxException (
				message: string.Concat ( "Order: " , exception.Message ) ,
				innerException: exception );
		}
	}

	public static IQueryable Select ( this IQueryable query , IProjectionQuery? projectionQuery )
	{
		NotNull ( query );
		NotNullOrEmpty ( projectionQuery?.Projection );

		try
		{
			return query.Select ( projectionQuery!.Projection! );
		}
		catch ( Exception exception )
		{
			throw new LinqSyntaxException (
				message: string.Concat ( "Projection: " , exception.Message ) ,
				innerException: exception );
		}
	}
}