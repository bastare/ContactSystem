namespace ContactSystem.Core.loC.Injectors.Interfaces;

public interface IInjector
{
	abstract static void Inject ( IServiceCollection serviceCollection );
}