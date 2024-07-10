namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Domain.Authorization.Session;
using Domain.Authorization.Session.Interfaces;
using Interfaces;

public sealed class SessionInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		serviceCollection.TryAddScoped<IUserSession , UserSession> ();
		serviceCollection.TryAddScoped<IUserSession<long> , UserSession> ();
	}
}