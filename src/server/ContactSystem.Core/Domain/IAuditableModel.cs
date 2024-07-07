namespace ContactSystem.Core.Domain;

public interface IAuditableModel<TKey> : IModel<TKey>, IAuditable<TKey>
	where TKey : struct
{ }