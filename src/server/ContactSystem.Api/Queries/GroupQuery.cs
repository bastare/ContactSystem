namespace ContactSystem.Api.Queries;

using Interfaces;

public sealed record GroupQuery : IGroupQuery
{
	public string? GroupBy { get; init; }
}