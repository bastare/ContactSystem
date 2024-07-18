namespace ContactSystem.Core.Domain.Pagination.Extensions;

using Pagination;

public static class PagedListExtensions
{
	public static PagedList<TTransformResult> Select<T, TTransformResult> ( this PagedList<T> pagedList , Func<T , TTransformResult> selector )
	{
		ArgumentNullException.ThrowIfNull ( pagedList );
		ArgumentNullException.ThrowIfNull ( selector );

		var transformedCollection =
			Enumerable.Select ( pagedList , selector );

		return PagedList<TTransformResult>.Create (
			transformedCollection ,
			pagedList.TotalCount ,
			pagedList.CurrentOffset ,
			pagedList.Limit );
	}
}