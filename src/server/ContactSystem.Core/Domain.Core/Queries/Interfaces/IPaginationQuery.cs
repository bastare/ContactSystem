namespace ContactSystem.Core.Domain.Core.Queries.Interfaces;

public interface IPaginationQuery
{
	long? Offset { get; }

	long? Limit { get; }
}