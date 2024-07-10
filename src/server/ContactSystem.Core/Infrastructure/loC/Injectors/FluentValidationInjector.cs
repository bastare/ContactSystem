namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Api.Endpoints.v1.Contracts.Validators;
using Domain.Core.Queries.Validators;
using Domain.Validators.Models;
using FluentValidation;
using Interfaces;

public sealed class FluentValidationInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		serviceCollection.AddValidatorsFromAssemblyContaining<PaginationQueryValidator> ();
	}
}