namespace ContactSystem.Core.Api.Endpoints.v1.Contracts;

using Domain.Core.Queries.Interfaces;

public sealed record GetContactsQuery (
	string? Expression ,
	string? Projection ,
	long? Offset ,
	long? Limit ,
	bool? IsDescending ,
	string? OrderBy ) :
		IExpressionQuery,
		IOrderQuery,
		IPaginationQuery,
		IProjectionQuery;