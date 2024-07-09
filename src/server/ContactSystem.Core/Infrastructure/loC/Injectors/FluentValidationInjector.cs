namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using Api.Queries.Validators;
using Interfaces;
using FluentValidation;

public sealed class FluentValidationInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		serviceCollection.AddValidatorsFromAssemblyContaining<PaginationQueryValidator> ();
	}
}