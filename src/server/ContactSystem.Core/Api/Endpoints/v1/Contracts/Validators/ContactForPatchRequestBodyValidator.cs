namespace ContactSystem.Core.Api.Endpoints.v1.Contracts.Validators;

using Domain.Validation;
using FluentValidation;

public sealed class ContactForPatchRequestBodyValidator : AbstractValidator<ContactForPatchRequestBody>
{
	public ContactForPatchRequestBodyValidator ()
	{
		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.FirstName )
			.NotEmpty ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.LastName )
			.NotEmpty ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.Email )
			.NotEmpty ()
			.EmailAddress ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.Phone )
			.NotEmpty ()
			.Phone ();

		RuleFor ( contactForPatchRequestBody => contactForPatchRequestBody.Title )
			.NotEmpty ();
	}
}