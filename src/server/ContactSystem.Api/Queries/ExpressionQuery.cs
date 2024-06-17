namespace ContactSystem.Api.Queries;

using Interfaces;

public sealed record ExpressionQuery : IExpressionQuery
{
	public string? Expression { get; init; }
}