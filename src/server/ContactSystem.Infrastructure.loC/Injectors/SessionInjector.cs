namespace ContactSystem.Infrastructure.loC.Injectors;

using Autofac;
using ContactSystem.Domain.Shared.Authorization.Session;
using Domain.Shared.Authorization.Session.Interfaces;

public sealed class SessionInjector : Module
{
	protected override void Load ( ContainerBuilder builder )
	{
		builder.RegisterGeneric ( typeof ( UserSession<> ) )
			.As ( typeof ( IUserSession<> ) )
			.InstancePerLifetimeScope ();

		builder.Register ( context => context.Resolve<IUserSession<int>> () )
			.As<IUserSession> ()
			.InstancePerLifetimeScope ();
	}
}