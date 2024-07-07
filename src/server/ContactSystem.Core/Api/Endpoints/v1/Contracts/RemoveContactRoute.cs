namespace ContactSystem.Core.Api.Endpoints.v1.Contracts;

public sealed record RemoveContactRoute
{
	public int Id { get; init; }
}