namespace ContactSystem.Infrastructure.loC.Injectors;

using Autofac;
using Domain.Shared.Authorization.Session;
using Domain.Shared.Authorization.Session.Interfaces;

public sealed class SessionInjector : Module
{
	protected override void Load ( ContainerBuilder builder )
	{
		builder.RegisterGeneric ( typeof ( UserSession<> ) )
			.As ( typeof ( IUserSession<> ) )
			.As ( typeof ( IUserSession ) )

			.InstancePerLifetimeScope ();
	}
}