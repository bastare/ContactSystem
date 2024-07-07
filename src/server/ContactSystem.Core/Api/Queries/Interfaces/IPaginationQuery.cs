namespace ContactSystem.Core.Api.Queries.Interfaces;

public interface IPaginationQuery
{
	long? Offset { get; }

	long? Limit { get; }
}