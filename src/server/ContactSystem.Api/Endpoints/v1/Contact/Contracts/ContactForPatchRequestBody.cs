namespace ContactSystem.Api.Endpoints.v1.Contact.Contracts;

public sealed record ContactForPatchRequestBody
{
	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public string? Email { get; set; }

	public string? Phone { get; set; }

	public string? Title { get; set; }

	public string? MiddleInitial { get; set; }
}