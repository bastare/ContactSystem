namespace ContactSystem.Core.Api.Endpoints.v1;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contracts;
using Core.Common.Exceptions;
using Decorators;
using Domain;
using Domain.Pagination;
using Domain.Pagination.Common.Extensions;
using Domain.Projections.Persistence;
using Domain.Projections.Persistence.Common.Extensions;
using Mapster;
using Specifications;
using Specifications.Evaluator.Common.Extensions;

public static class ContactEndpoints
{
	public static async Task<IResult> GetAllAsync (
		[FromServices] IContactSet contactSet ,
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
			=> contactSet.Contacts
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
		[FromServices] IContactSet contactSet ,
		[FromBody] ContactForCreationRequestBody contactForCreationRequestBody ,
		CancellationToken cancellationToken = default )
	{
		var createdContact_ =
			await CreateContactsAsync ( contactForCreationRequestBody );

		await contactSet.SaveChangesAsync ( cancellationToken );

		return Results.Created (
			$"/v1/contacts/{createdContact_.Id}" ,
			createdContact_ );

		async Task<Contact> CreateContactsAsync ( ContactForCreationRequestBody contactForCreationRequestBody )
		{
			var modelContactForCreation_ = contactForCreationRequestBody.Adapt<Contact> ();

			await contactSet.Contacts
				.AddAsync (
					modelContactForCreation_ ,
					cancellationToken );

			return modelContactForCreation_;
		}
	}

	public static async Task<IResult> PatchAsync (
		[FromServices] IContactSet contactSet ,
		[FromRoute] long contactId ,
		[FromBody] ContactForPatchRequestBody contactForPatchRequestBody ,
		CancellationToken cancellationToken = default )
	{
		var patchedContact_ =
			await PatchContactAsync ( contactId , contactForPatchRequestBody );

		await contactSet.TryCommitAsync ( cancellationToken );

		return Results.Ok ( patchedContact_ );

		async Task<Contact> PatchContactAsync ( long contactId , ContactForPatchRequestBody contactForPatchRequestBody )
		{
			// TODO: Integrate `ExecuteUpdate`
			var config_ = new TypeAdapterConfig ();
			config_.Default.IgnoreNullValues ( true );

			var contactForUpdate_ =
				await contactSet.Contacts.FindAsync ( [ contactId ] , cancellationToken: cancellationToken ) ??
					throw new NotFoundException ( message: $"There is no `Contact` with this id: {contactId}" );

			return contactForPatchRequestBody.Adapt ( contactForUpdate_ , config_ )!;
		}
	}

	public static async Task<IResult> RemoveAsync (
		[FromServices] IContactSet contactSet ,
		[FromRoute] long contactId ,
		CancellationToken cancellationToken = default )
	{
		await RemoveContactsAsync ( contactId );

		await contactSet.TryCommitAsync ( cancellationToken );

		return Results.NoContent ();

		Task RemoveContactsAsync ( long contactId )
			=> contactSet.Contacts
				.Where ( contact => contact.Id == contactId )
				.ExecuteDeleteAsync ( cancellationToken );
	}
}