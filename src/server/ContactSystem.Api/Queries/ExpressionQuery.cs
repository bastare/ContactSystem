namespace ContactSystem.Api.Queries;

using Interfaces;
using Microsoft.AspNetCore.Mvc;

public sealed record ExpressionQuery : IExpressionQuery
{
	[FromQuery ( Name = "expression" )]
	public string? Expression { get; init; }
}