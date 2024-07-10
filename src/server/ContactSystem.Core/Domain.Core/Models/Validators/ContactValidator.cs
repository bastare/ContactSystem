namespace ContactSystem.Core.Domain.Core.Models.Validators;

using Validation;
using FluentValidation;
using Core.Models;

public sealed class CustomerValidator : AbstractValidator<Contact>
{
	public CustomerValidator ()
	{
		RuleFor ( contact => contact.FirstName )
			.NotEmpty ();

		RuleFor ( contact => contact.LastName )
			.NotEmpty ();

		RuleFor ( contact => contact.Email )
			.NotEmpty ()
			.EmailAddress ();

		RuleFor ( contact => contact.Phone )
			.NotEmpty ()
			.Phone ();

		RuleFor ( contact => contact.Title )
			.NotEmpty ();
	}
}