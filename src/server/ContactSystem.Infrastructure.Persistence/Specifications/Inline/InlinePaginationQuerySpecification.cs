namespace ContactSystem.Infrastructure.Persistence.Specifications.Inline;

using Domain.Core.Models;
using Domain.Contracts.Dtos.QueryDtos;
using DynamicLinqDecorator;
using Microsoft.EntityFrameworkCore.Query;
using System;

public sealed record InlinePaginationQuerySpecification<TModel, TKey> :
	IPaginationQuerySpecification<TModel , TKey>,
	IQuerySpecification<TModel , TKey>,
	IProjectionQuerySpecification<TModel , TKey>
		where TModel : IModel<TKey>
{
	public Func<IQueryable<TModel> , IQueryable<TModel>>? Conditions { get; init; }

	public Func<IQueryable<TModel> , IIncludableQueryable<TModel , object>>? Includes { get; init; }

	public Func<IQueryable<TModel> , IOrderedQueryable<TModel>>? OrderBy { get; init; }

	public Func<IQueryable , IQueryable>? Projection { get; init; }

	public int Limit { get; init; }

	public int Offset { get; init; }

	public InlinePaginationQuerySpecification (
		ExpressionQueryDto? expressionQuery = default ,
		OrderQueryDto? orderQuery = default ,
		PaginationQueryDto? paginationQuery = default ,
		ProjectionQueryDto? projectionQuery = default )
	{
		if ( expressionQuery is { Expression: not null } )
			Conditions = query =>
				query.Where ( expressionQuery );

		if ( orderQuery is { OrderBy: not null, IsDescending: not null } )
			OrderBy = query =>
				query.OrderBy ( orderQuery );

		if ( projectionQuery is { Projection: not null } )
			Projection = query =>
				query.Select ( projectionQuery );

		if ( paginationQuery is not null )
		{
			Offset = paginationQuery.Offset;
			Limit = paginationQuery.Limit;
		}
	}
}