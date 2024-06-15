namespace ContactSystem.Api.Queries;

using Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public sealed record PaginationQuery : IPaginationQuery
{
	[Range ( 1 , int.MaxValue )]
	[FromQuery ( Name = "offset" )]
	public int? Offset { get; init; } = 1;

	[Range ( 1 , int.MaxValue )]
	[FromQuery ( Name = "limit" )]
	public int? Limit { get; init; } = 10;
}