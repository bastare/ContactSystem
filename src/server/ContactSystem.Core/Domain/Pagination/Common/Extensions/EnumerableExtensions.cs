namespace ContactSystem.Core.Domain.Pagination.Common.Extensions;

using Domain.Pagination;

public static class EnumerableExtensions
{
	public static PagedList<T> ToPagedList<T> ( this IEnumerable<T> collection , long count , long offset , long limit )
		=> PagedList<T>.Create ( collection , count , offset , limit );
}