namespace ContactSystem.Core.Domain.Projections.Persistence.Common.Extensions;

using Core.Common.Exceptions;
using Transactions;

public static class DbContextExtensions
{
	public static async Task CommitAsync ( this ITransaction transaction , CancellationToken cancellationToken = default )
	{
		if ( await transaction.TryCommitAsync ( cancellationToken ) )
			return;

		throw new DataWasNotSavedException ();
	}

	public static async Task<bool> TryCommitAsync ( this ITransaction transaction , CancellationToken cancellationToken = default )
		=> await transaction.SaveChangesAsync ( cancellationToken ) != 0;
}