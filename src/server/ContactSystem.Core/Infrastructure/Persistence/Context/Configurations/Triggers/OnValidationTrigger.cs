namespace ContactSystem.Core.Infrastructure.Persistence.Context.Configurations.Triggers;

using EntityFrameworkCore.Triggered;
using Domain.Core;
using Domain.Validation.Common.Exceptions;
using FluentValidation.Results;

public sealed class OnValidationTrigger :
	IBeforeSaveTrigger<IHasValidation>,
	IBeforeSaveTrigger<IHasValidationAsync>
{
	public async Task BeforeSave ( ITriggerContext<IHasValidationAsync> context , CancellationToken cancellationToken )
	{
		ValidateAndThrow (
			validationResult: await context.Entity.ValidateAsync ( cancellationToken ) );
	}

	public Task BeforeSave ( ITriggerContext<IHasValidation> context , CancellationToken _ )
	{
		ValidateAndThrow (
			validationResult: context.Entity.Validate () );

		return Task.CompletedTask;
	}

	private static void ValidateAndThrow ( ValidationResult validationResult )
	{
		if ( validationResult.IsValid )
			return;

		throw new ValidationFailureException ( validationResult );
	}
}