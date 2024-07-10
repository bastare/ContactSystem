namespace ContactSystem.Core.Specifications.Evaluator.Common.Extensions;

using Domain;
using Specifications;

public static class QueryableExtensions
{
	public static IQueryable SpecifiedQuery ( this IQueryable inputQuery , InlineQuerySpecification inlineSpecification )
	{
		NotNull ( inputQuery );
		NotNull ( inlineSpecification?.QueryInjector );

		return inlineSpecification!.QueryInjector!.Invoke ( inputQuery );
	}

	public static IQueryable SpecifiedQuery<TModel> ( this IQueryable<TModel> typedInputQuery , InlineQuerySpecification inlineSpecification )
		=> SpecifiedQuery (
			inputQuery: typedInputQuery ,
			inlineSpecification );

	public static IQueryable<TModel> SpecifiedQuery<TModel, TKey> ( this IQueryable<TModel> typedInputQuery , QuerySpecification<TModel , TKey> querySpecification )
		where TModel : class, IModel<TKey>
	{
		NotNull ( typedInputQuery );
		NotNull ( querySpecification?.QueryInjector );

		return querySpecification!.QueryInjector!.Invoke ( typedInputQuery );
	}
}