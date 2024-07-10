namespace ContactSystem.Core.Domain.Validation.Common.Exceptions
{
	using FluentValidation.Results;

	public sealed class ValidationFailureException ( ValidationResult failedValidationResult ) : Exception
	{
		public ValidationResult FailedValidationResult { get; } = failedValidationResult;
	}
}