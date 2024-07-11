namespace ContactSystem.Core.Domain.Core.Queries.Interfaces;

public interface IPaginationQuery
{
	int? Offset { get; }

	int? Limit { get; }
}