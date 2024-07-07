namespace ContactSystem.Core.Infrastructure.Persistence.Pagination.Interfaces;

public interface IPagedList : IEnumerable
{
	long CurrentOffset { get; }

	long TotalPages { get; }

	long Limit { get; }

	long TotalCount { get; }
}

public interface IPagedList<out T> : IPagedList, IEnumerable<T>
{ }