namespace ContactSystem.Domain.Shared.Authorization.Session.Interfaces;

public interface IUserSession
{
	Guid? Id { get; }

	bool IsAuthorizedUser ();
}