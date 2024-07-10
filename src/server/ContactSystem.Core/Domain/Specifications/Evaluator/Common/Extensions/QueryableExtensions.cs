namespace ContactSystem.Core.Domain.Specifications.Evaluator.Common.Extensions;

using Core;

public static class QueryableExtensions
{
	public static IQueryable SpecifiedQuery ( this IQueryable inputQuery , InlineQuerySpecification inlineSpecification )
	{
		NotNull ( inputQuery );
		NotNull ( inlineSpecification?.QueryInjector );

		return inlineSpecification!.QueryInjector!.Invoke ( inputQuery );
	}

	public static IQueryable<TModel> SpecifiedQuery<TModel, TKey> ( this IQueryable<TModel> typedInputQuery , QuerySpecification<TModel , TKey> querySpecification )
		where TModel : class, IModel<TKey>
	{
		NotNull ( typedInputQuery );
		NotNull ( querySpecification?.QueryInjector );

		return querySpecification!.QueryInjector!.Invoke ( typedInputQuery );
	}
}