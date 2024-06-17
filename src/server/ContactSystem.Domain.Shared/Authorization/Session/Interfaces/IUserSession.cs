namespace ContactSystem.Domain.Shared.Authorization.Session.Interfaces;

public interface IUserSession
{
	int? Id { get; }

	bool IsAuthorizedUser ();
}