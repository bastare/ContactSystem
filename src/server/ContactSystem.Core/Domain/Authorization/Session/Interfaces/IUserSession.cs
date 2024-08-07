namespace ContactSystem.Core.Domain.Authorization.Session.Interfaces;

public interface IUserSession<TKey> : IUserSession
{
	new TKey Id { get; }
}

public interface IUserSession
{
	object Id { get; }

	bool IsAuthorizedUser ();
}