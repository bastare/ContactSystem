namespace ContactSystem.Core.Api.Endpoints.v1.Contracts;

public sealed record ContactForCreationRequestBody
{
	public string? FirstName { get; init; }

	public string? LastName { get; init; }

	public string? Email { get; init; }

	public string? Phone { get; init; }

	public string? Title { get; init; }

	public string? MiddleInitial { get; init; }
};