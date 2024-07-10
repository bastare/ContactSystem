namespace ContactSystem.Core.Domain.Core;

using FluentValidation.Results;

public interface IHasValidationAsync
{
	Task<ValidationResult> ValidateAsync ( CancellationToken cancellationToken = default );

	Task ValidateAndThrowAsync ( CancellationToken cancellationToken = default );

	Task<bool> IsValidAsync ( CancellationToken cancellationToken = default );
}