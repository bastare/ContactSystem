namespace ContactSystem.Api.Endpoints.v1.Contact.Contracts;

public sealed record RemoveContactRoute
{
	public int Id { get; init; }
}