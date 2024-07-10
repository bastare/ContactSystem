namespace ContactSystem.Core.Infrastructure.Persistence.Context.Configurations.Triggers;

using EntityFrameworkCore.Triggered;
using Domain.Core;
using Domain.Validation.Common.Exceptions;

public sealed class OnValidationTrigger : IBeforeSaveTrigger<IHasValidationAsync>
{
	public async Task BeforeSave ( ITriggerContext<IHasValidationAsync> context , CancellationToken cancellationToken )
	{
		var validationResult =
			await context.Entity.ValidateAsync ( cancellationToken );

		if ( validationResult.IsValid )
			return;

		throw new ValidationFailureException ( validationResult );
	}
}