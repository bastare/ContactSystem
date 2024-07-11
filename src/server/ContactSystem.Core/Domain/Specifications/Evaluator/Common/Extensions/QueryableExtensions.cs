namespace ContactSystem.Core.Domain.Specifications.Evaluator.Common.Extensions;

using Interfaces;
using Core;

public static class QueryableExtensions
{
	public static IQueryable SpecifiedQuery ( this IQueryable inputQuery , IQuerySpecification querySpecification )
	{
		NotNull ( inputQuery );
		NotNull ( querySpecification?.QueryInjector );

		return querySpecification!.QueryInjector!.Invoke ( inputQuery );
	}

	public static IQueryable<TModel> SpecifiedQuery<TModel, TKey> ( this IQueryable<TModel> typedInputQuery , IQuerySpecification<TModel , TKey> querySpecification )
		where TModel : class, IModel<TKey>
	{
		NotNull ( typedInputQuery );
		NotNull ( querySpecification?.QueryInjector );

		return querySpecification!.QueryInjector!.Invoke ( typedInputQuery );
	}
}