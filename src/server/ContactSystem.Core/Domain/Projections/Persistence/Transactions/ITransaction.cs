namespace ContactSystem.Core.Domain.Projections.Persistence.Transactions;

public interface ITransaction
{
	Task<int> SaveChangesAsync ( CancellationToken cancellationToken = default );
}