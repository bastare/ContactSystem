namespace ContactSystem.Api.Queries;

using Interfaces;
using Microsoft.AspNetCore.Mvc;

public sealed record GroupQuery : IGroupQuery
{
	[FromQuery ( Name = "group_by" )]
	public string? GroupBy { get; init; }
}