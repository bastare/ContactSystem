namespace ContactSystem.Core.Persistence.Specifications;

using Persistence.Specifications.Interfaces;
using Domain.Core.Models;

public abstract record QuerySpecification<TModel, TKey> : IQuerySpecification<TModel , TKey>
	where TModel : IModel<TKey>
{
	public Func<IQueryable<TModel> , IQueryable<TModel>>? QueryInjector { get; protected init; }

	Func<IQueryable , IQueryable>? IQuerySpecification.QueryInjector => ( Func<IQueryable , IQueryable>? ) QueryInjector;
}