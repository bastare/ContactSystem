namespace ContactSystem.Core.Domain.Pagination.Common.Extensions;

using Domain.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

public static class QueryableExtensions
{
	public async static Task<PagedList<T>> ToPagedListAsync<T> ( this IQueryable<T> queryable , int offset , int limit , CancellationToken cancellationToken = default )
		where T : class
	{
		var count = await GetCountOfTableRecordsAsync ( queryable , cancellationToken );

		var pagedData =
			await GetPagedRecords ( queryable , offset , limit )
				.ToListAsync ( cancellationToken );

		return PagedList<T>.Create ( pagedData , count , offset , limit );

		static Task<int> GetCountOfTableRecordsAsync ( IQueryable<T> queryable , CancellationToken cancellationToken = default )
			=> queryable.CountAsync ( cancellationToken );
	}

	public async static Task<PagedList<object>> ToPagedListAsync (
		this IQueryable queryable ,
		int offset ,
		int limit ,
		CancellationToken cancellationToken = default )
	{
		var count = GetCountOfTableRecords ( queryable );

		var pagedData =
			await GetPagedRecords ( queryable , offset , limit )
				.ToDynamicListAsync ( cancellationToken );

		return PagedList<object>.Create ( pagedData , count , offset , limit );

		static int GetCountOfTableRecords ( IQueryable queryable )
			=> queryable.Count ();
	}

	private static IQueryable GetPagedRecords ( IQueryable queryable , int offset , int limit )
		=> offset switch
		{
			>= 1 =>
				queryable
					.Skip ( ( offset - 1 ) * limit )
					.Take ( limit ),

			<= 0 =>
				throw new ArgumentException ( "Offset can`t be a negative number" , nameof ( offset ) )
		};

	private static IQueryable<T> GetPagedRecords<T> ( IQueryable<T> queryable , int offset , int limit )
		where T : class
			=> ( IQueryable<T> ) GetPagedRecords ( ( IQueryable ) queryable , offset , limit );
}