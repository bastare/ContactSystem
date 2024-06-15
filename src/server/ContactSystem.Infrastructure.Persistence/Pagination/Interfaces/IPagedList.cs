namespace ContactSystem.Infrastructure.Persistence.Pagination.Interfaces;

public interface IPagedList : IEnumerable
{
	int CurrentOffset { get; }

	int TotalPages { get; }

	int Limit { get; }

	int TotalCount { get; }
}

public interface IPagedList<out T> : IPagedList, IEnumerable<T>
{ }