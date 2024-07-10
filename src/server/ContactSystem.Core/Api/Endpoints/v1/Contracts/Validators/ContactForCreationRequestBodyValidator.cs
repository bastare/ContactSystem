namespace ContactSystem.Core.Api.Endpoints.v1.Contracts.Validators;

using Domain.Validation;
using FluentValidation;

public sealed class ContactForCreationRequestBodyValidator : AbstractValidator<ContactForCreationRequestBody>
{
	public ContactForCreationRequestBodyValidator ()
	{
		RuleFor ( contactForCreationRequestBody => contactForCreationRequestBody.FirstName )
			.NotEmpty ();

		RuleFor ( contactForCreationRequestBody => contactForCreationRequestBody.LastName )
			.NotEmpty ();

		RuleFor ( contactForCreationRequestBody => contactForCreationRequestBody.Email )
			.NotEmpty ()
			.EmailAddress ();

		RuleFor ( contactForCreationRequestBody => contactForCreationRequestBody.Phone )
			.NotEmpty ()
			.Phone ();

		RuleFor ( contactForCreationRequestBody => contactForCreationRequestBody.Title )
			.NotEmpty ();
	}
}