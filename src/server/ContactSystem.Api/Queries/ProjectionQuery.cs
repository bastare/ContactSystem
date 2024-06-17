namespace ContactSystem.Api.Queries;

using Interfaces;
using Microsoft.AspNetCore.Mvc;

public sealed record ProjectionQuery : IProjectionQuery
{
	public string? Projection { get; init; }
}