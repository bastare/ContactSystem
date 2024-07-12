namespace ContactSystem.Core.Domain.Specifications.Interfaces;

using Core;

public abstract record QuerySpecification<TModel, TKey> : IQuerySpecification<TModel , TKey>
	where TModel : IModel<TKey>
{
	public Func<IQueryable<TModel> , IQueryable<TModel>>? QueryInjector { get; protected init; }

	Func<IQueryable , IQueryable>? IQuerySpecification.QueryInjector => ( Func<IQueryable , IQueryable>? ) QueryInjector;
}

public abstract record QuerySpecification : IQuerySpecification
{
	public Func<IQueryable , IQueryable>? QueryInjector { get; protected init; }
}