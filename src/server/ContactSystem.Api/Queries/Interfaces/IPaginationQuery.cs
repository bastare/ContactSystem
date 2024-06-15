namespace ContactSystem.Api.Queries.Interfaces;

public interface IPaginationQuery
{
	int? Offset { get; }

	int? Limit { get; }
}