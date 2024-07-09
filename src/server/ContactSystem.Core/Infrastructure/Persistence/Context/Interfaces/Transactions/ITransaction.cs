namespace ContactSystem.Core.Infrastructure.Persistence.Context.Interfaces.Transactions;

public interface ITransaction
{
	Task<int> SaveChangesAsync ( CancellationToken cancellationToken = default );
}