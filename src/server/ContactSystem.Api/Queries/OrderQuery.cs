namespace ContactSystem.Api.Queries;

using Interfaces;
using Microsoft.AspNetCore.Mvc;

public sealed record OrderQuery : IOrderQuery
{
	[FromQuery ( Name = "is_descending" )]
	public bool IsDescending { get; init; }

	[FromQuery ( Name = "order_by" )]
	public string? OrderBy { get; init; }
}