namespace ContactSystem.Core.Infrastructure.Persistence.Specifications;

using Domain;
using Interfaces;

public abstract record QuerySpecification<TModel, TKey> : IQuerySpecification<TModel , TKey>
	where TModel : IModel<TKey>
{
	public Func<IQueryable<TModel> , IQueryable<TModel>>? QueryInjector { get; protected init; }

	Func<IQueryable , IQueryable>? IQuerySpecification.QueryInjector => ( Func<IQueryable , IQueryable>? ) QueryInjector;
}