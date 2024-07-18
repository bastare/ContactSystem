namespace ContactSystem.Core.Domain.Specifications.Evaluator.Common.Extensions;

using Interfaces;
using Core;

public static class QueryableExtensions
{
	public static IQueryable SpecifiedQuery ( this IQueryable inputQuery , IQuerySpecification querySpecification )
	{
		ArgumentNullException.ThrowIfNull ( inputQuery );
		ArgumentNullException.ThrowIfNull ( querySpecification?.QueryInjector );

		return querySpecification!.QueryInjector!.Invoke ( inputQuery );
	}

	public static IQueryable<TModel> SpecifiedQuery<TModel, TKey> ( this IQueryable<TModel> typedInputQuery , IQuerySpecification<TModel , TKey> querySpecification )
		where TModel : class, IModel<TKey>
	{
		ArgumentNullException.ThrowIfNull ( typedInputQuery );
		ArgumentNullException.ThrowIfNull ( querySpecification?.QueryInjector );

		return querySpecification!.QueryInjector!.Invoke ( typedInputQuery );
	}
}