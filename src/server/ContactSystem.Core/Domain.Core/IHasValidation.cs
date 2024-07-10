namespace ContactSystem.Core.Domain.Core;

using FluentValidation.Results;

public interface IHasValidation
{
	ValidationResult Validate ();

	void ValidateAndThrow ();

	bool IsValid ();
}