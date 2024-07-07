namespace ContactSystem.Core.Persistence.Common.Extensions;

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

	public static async Task CommitAsync ( this DbContext dbContext , CancellationToken cancellationToken = default )
	{
		if ( await dbContext.TryCommitAsync ( cancellationToken ) )
			return;

		throw new DataWasNotSavedException ();
	}

	public static async Task<bool> TryCommitAsync ( this DbContext dbContext , CancellationToken cancellationToken = default )
		=> await dbContext.SaveChangesAsync ( cancellationToken ) != 0;
}