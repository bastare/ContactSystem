namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using Api.Queries.Validators;
using FluentValidation;
using Interfaces;

public sealed class FluentValidationInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		serviceCollection.AddValidatorsFromAssemblyContaining<PaginationQueryValidator> ();
	}
}