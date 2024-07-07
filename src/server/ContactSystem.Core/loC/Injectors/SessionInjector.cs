namespace ContactSystem.Core.loC.Injectors;

using Interfaces;
using Domain.Authorization.Session;
using Domain.Authorization.Session.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class SessionInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		serviceCollection.TryAddScoped<IUserSession , UserSession> ();
		serviceCollection.TryAddScoped<IUserSession<long> , UserSession> ();
	}
}