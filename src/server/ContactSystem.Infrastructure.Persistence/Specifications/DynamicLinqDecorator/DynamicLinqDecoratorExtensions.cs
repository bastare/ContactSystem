namespace ContactSystem.Infrastructure.Persistence.Specifications.DynamicLinqDecorator;

using System.Linq.Dynamic.Core;
using Domain.Contracts.Dtos.QueryDtos;

public static class DynamicLinqDecoratorExtensions
{
	public static IQueryable<TModel> Where<TModel> ( this IQueryable<TModel> query , ExpressionQueryDto? expressionQuery )
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

	public static IOrderedQueryable<TModel> OrderBy<TModel> ( this IQueryable<TModel> query , OrderQueryDto? orderQuery )
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

	public static IQueryable Select ( this IQueryable query , ProjectionQueryDto? projectionQuery )
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