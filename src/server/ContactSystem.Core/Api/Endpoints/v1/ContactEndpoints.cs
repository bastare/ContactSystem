namespace ContactSystem.Core.Api.Endpoints.v1;

using Contracts;
using Microsoft.AspNetCore.Mvc;
using Domain.Contact;
using Mapster;
using Core.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Decorators;
using Infrastructure.Persistence.Common.Extensions;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Pagination;
using Infrastructure.Persistence.Specifications.Evaluator.Common.Extensions;
using Infrastructure.Persistence.Specifications;

public static class ContactEndpoints
{
	public static async Task<IResult> GetAllAsync (
		[FromServices] EfContext efContext ,
		[AsParameters] GetContactsQuery getContactsQuery ,
		CancellationToken cancellationToken = default )
	{
		var contacts_ = await GetContactsAsync ( getContactsQuery );

		return Results.Ok (
			value: new PaginationRowsDecoratorDto<object>
			{
				Rows = contacts_ ,
				TotalPages = contacts_.TotalPages ,
				CurrentOffset = contacts_.CurrentOffset ,
				TotalCount = contacts_.TotalCount

			} );

		Task<PagedList<object>> GetContactsAsync ( GetContactsQuery getContactsQuery )
			=> efContext.Contacts
				.SpecifiedQuery (
					inlineSpecification: new InlineQuerySpecification (
						getContactsQuery ,
						getContactsQuery ,
						getContactsQuery ) )

				.ToPagedListAsync (
					getContactsQuery?.Offset ?? 1 ,
					getContactsQuery?.Limit ?? 10 ,
					cancellationToken );
	}

	public static async Task<IResult> CreateAsync (
		[FromServices] EfContext efContext ,
		[FromBody] ContactForCreationRequestBody contactForCreationRequestBody ,
		CancellationToken cancellationToken = default )
	{
		var createdContact_ =
			await CreateContactsAsync ( contactForCreationRequestBody );

		await efContext.SaveChangesAsync ( cancellationToken );

		return Results.Created (
			$"/v1/contacts/{createdContact_.Id}" ,
			createdContact_ );

		async Task<Contact> CreateContactsAsync ( ContactForCreationRequestBody contactForCreationRequestBody )
		{
			var modelContactForCreation_ = contactForCreationRequestBody.Adapt<Contact> ();

			await efContext.Contacts
				.AddAsync (
					modelContactForCreation_ ,
					cancellationToken );

			return modelContactForCreation_;
		}
	}

	public static async Task<IResult> PatchAsync (
		[FromRoute] long contactId ,
		[FromServices] EfContext efContext ,
		[FromBody] ContactForPatchRequestBody contactForPatchRequestBody ,
		CancellationToken cancellationToken = default )
	{
		var patchedContact_ =
			await PatchContactAsync ( contactId , contactForPatchRequestBody );

		await efContext.TryCommitAsync ( cancellationToken );

		return Results.Ok ( patchedContact_ );

		async Task<Contact> PatchContactAsync ( long contactId , ContactForPatchRequestBody contactForPatchRequestBody )
		{
			// TODO: Integrate `ExecuteUpdate`
			var config = new TypeAdapterConfig ();
			config.Default.IgnoreNullValues ( true );

			var contactForUpdate_ =
				await efContext.Contacts.FindAsync ( [ contactId ] , cancellationToken: cancellationToken ) ??
					throw new NotFoundException ( message: $"There is no `Contact` with this id: {contactId}" );

			return contactForPatchRequestBody.Adapt ( contactForUpdate_ , config )!;
		}
	}

	public static async Task<IResult> RemoveAsync (
		[FromRoute] long contactId ,
		[FromServices] EfContext efContext ,
		CancellationToken cancellationToken = default )
	{
		await RemoveContactsAsync ( contactId );

		await efContext.TryCommitAsync ( cancellationToken );

		return Results.NoContent ();

		Task RemoveContactsAsync ( long contactId )
			=> efContext.Contacts
				.Where ( contact => contact.Id == contactId )
				.ExecuteDeleteAsync ( cancellationToken );
	}
}