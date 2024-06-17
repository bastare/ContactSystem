namespace ContactSystem.Api.Queries.Interfaces;

using Microsoft.AspNetCore.Mvc;

public interface IGroupQuery
{
	[FromQuery ( Name = "group_by" )]
	string? GroupBy { get; }
}