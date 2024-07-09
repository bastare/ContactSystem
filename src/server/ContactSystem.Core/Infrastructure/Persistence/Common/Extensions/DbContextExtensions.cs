namespace ContactSystem.Core.Infrastructure.Persistence.Common.Extensions;

using Context;
using Context.Interfaces.Transactions;
using Exceptions;
using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
	public static IQueryable<TModel> Set<TModel> ( this DbContext dbContext , bool isTracking = default )
		where TModel : class
			=> isTracking
				? dbContext.Set<TModel> ()

				: dbContext.Set<TModel> ()
					.AsNoTracking ();

	public static async Task CommitAsync ( this ITransaction transaction , CancellationToken cancellationToken = default )
	{
		if ( await transaction.TryCommitAsync ( cancellationToken ) )
			return;

		throw new DataWasNotSavedException ();
	}

	public static async Task<bool> TryCommitAsync ( this ITransaction transaction , CancellationToken cancellationToken = default )
		=> await transaction.SaveChangesAsync ( cancellationToken ) != 0;

	public static Task CommitAsync ( this EfContext dbContext , CancellationToken cancellationToken = default )
		=> CommitAsync (
			transaction: dbContext ,
			cancellationToken );

	public static Task<bool> TryCommitAsync ( this EfContext dbContext , CancellationToken cancellationToken = default )
		=> TryCommitAsync (
			transaction: dbContext ,
			cancellationToken );
}