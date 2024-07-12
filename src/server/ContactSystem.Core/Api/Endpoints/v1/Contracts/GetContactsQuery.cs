namespace ContactSystem.Core.Api.Endpoints.v1.Contracts;

using Domain.Core.Queries.Interfaces;

public sealed record GetContactsQuery :
	IExpressionQuery,
	IOrderQuery,
	IPaginationQuery,
	IProjectionQuery
{
	public int? Offset { get; init; }

	public int? Limit { get; init; }

	public bool? IsDescending { get; init; }

	public string? OrderBy { get; init; }

	public string? Projection { get; init; }

	public string? Expression { get; init; }
}