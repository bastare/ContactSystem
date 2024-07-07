namespace ContactSystem.Core.Domain.Authorization.Session;

using Interfaces;
using Microsoft.AspNetCore.Http;

public sealed class UserSession ( IHttpContextAccessor httpContextAccessor ) : IUserSession<long>
{
	private readonly IHttpContextAccessor _ = httpContextAccessor;

	public long Id => default!;

	object IUserSession.Id => Id!;

	public bool IsAuthorizedUser ()
		=> false;
}