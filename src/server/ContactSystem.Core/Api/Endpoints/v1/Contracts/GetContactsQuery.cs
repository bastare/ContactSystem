namespace ContactSystem.Core.Api.Endpoints.v1.Contracts;

using Domain.Core.Queries.Interfaces;

public sealed record GetContactsQuery (
	string? Expression ,
	string? Projection ,
	int? Offset ,
	int? Limit ,
	bool? IsDescending ,
	string? OrderBy ) :
		IExpressionQuery,
		IOrderQuery,
		IPaginationQuery,
		IProjectionQuery;