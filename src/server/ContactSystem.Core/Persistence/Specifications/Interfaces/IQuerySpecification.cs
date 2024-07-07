namespace ContactSystem.Core.Persistence.Specifications.Interfaces;

using Domain;

public interface IQuerySpecification<TModel, TKey> : IQuerySpecification
	where TModel : IModel<TKey>
{
	new Func<IQueryable<TModel> , IQueryable<TModel>>? QueryInjector { get; }
}

public interface IQuerySpecification
{
	Func<IQueryable , IQueryable>? QueryInjector { get; }
}