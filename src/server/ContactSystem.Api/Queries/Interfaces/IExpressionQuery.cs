namespace ContactSystem.Api.Queries.Interfaces;

using Microsoft.AspNetCore.Mvc;

public interface IExpressionQuery
{
	[FromQuery ( Name = "expression" )]
	string? Expression { get; }
}