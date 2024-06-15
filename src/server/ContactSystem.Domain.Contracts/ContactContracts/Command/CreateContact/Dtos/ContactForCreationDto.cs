namespace ContactSystem.Domain.Contracts.ContactContracts.Command.CreateContact.Dtos;

public sealed record ContactForCreationDto
{
	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public string? Email { get; set; }

	public string? Phone { get; set; }

	public string? Title { get; set; }

	public string? MiddleInitial { get; set; }
}