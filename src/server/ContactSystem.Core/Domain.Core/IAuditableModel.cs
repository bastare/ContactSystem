namespace ContactSystem.Core.Domain.Core;

public interface IAuditableModel<TKey> : IModel<TKey>, IAuditable<TKey>
	where TKey : struct
{ }