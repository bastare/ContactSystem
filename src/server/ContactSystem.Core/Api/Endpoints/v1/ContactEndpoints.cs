namespace ContactSystem.Core.Api.Endpoints.v1;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contracts;
using Core.Common.Exceptions;
using Decorators;
using Domain.Core.Models;
using Domain.Pagination;
using Domain.Pagination.Common.Extensions;
using Domain.Projections.Persistence;
using Domain.Projections.Persistence.Common.Extensions;
using Domain.Specifications.Evaluator.Common.Extensions;
using Domain.Specifications;
using Mapster;
using Interfaces;
using ContactSystem.Core.Common.Classes.HttpMessages.Error;

public sealed class ContactEndpoints : IHasEndpoints
{
	public static void MapEndpoints ( RouteGroupBuilder routeGroupBuilder )
	{
		var v1Contacts =
			routeGroupBuilder
				.MapGroup ( "contacts" )
				.MapToApiVersion ( 1.0 );

		v1Contacts.MapGet (
			pattern: string.Empty ,
			handler: GetAllAsync );

		v1Contacts.MapPost (
			pattern: string.Empty ,
			handler: CreateAsync );

		v1Contacts.MapDelete (
			pattern: "{contactId:long}" ,
			handler: RemoveAsync );

		v1Contacts.MapPatch (
			pattern: "{contactId:long}" ,
			handler: PatchAsync );
	}

	public static async Task<IResult> GetAllAsync (
		[FromServices] IContactSet contactSet ,
		[AsParameters] GetContactsQuery getContactsQuery ,
		CancellationToken cancellationToken )
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
					querySpecification: new DynamicQuerySpecification ( getContactsQuery ) )

				.ToPagedListAsync (
					getContactsQuery?.Offset ?? 1 ,
					getContactsQuery?.Limit ?? 10 ,
					cancellationToken );
	}

	public static async Task<IResult> CreateAsync (
		[FromServices] IContactSet contactSet ,
		[FromBody] ContactForCreationRequestBody contactForCreationRequestBody ,
		CancellationToken cancellationToken )
	{
		var createdContact_ =
			await CreateContactAsync ( contactForCreationRequestBody );

		await contactSet.CommitAsync ( cancellationToken );

		return Results.Created (
			uri: $"/v1/contacts/{createdContact_.Id}" ,
			createdContact_ );

		async Task<Contact> CreateContactAsync ( ContactForCreationRequestBody contactForCreationRequestBody )
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
		CancellationToken cancellationToken )
	{
		var patchedContact_ =
			await PatchContactAsync ( contactId , contactForPatchRequestBody );

		if ( patchedContact_ is null )
			return Results.NotFound (
				new PageErrorMessage (
					StatusCode: StatusCodes.Status404NotFound ,
					Message: $"There is no `Contact` with this id: {contactId}" ,
					Description: "Sorry, the page you are looking for does not exist." ) );

		await contactSet.TryCommitAsync ( cancellationToken );

		return Results.Ok ( patchedContact_ );

		async Task<Contact?> PatchContactAsync ( long contactId , ContactForPatchRequestBody contactForPatchRequestBody )
		{
			// TODO: Integrate `ExecuteUpdate`
			var config_ = new TypeAdapterConfig ();
			config_.Default.IgnoreNullValues ( true );

			var contactForUpdate_ =
				await contactSet.Contacts.FindAsync ( [ contactId ] , cancellationToken: cancellationToken );

			return contactForUpdate_ is not null
				? contactForPatchRequestBody.Adapt ( contactForUpdate_ , config_ )
				: null;
		}
	}

	public static async Task<IResult> RemoveAsync (
		[FromServices] IContactSet contactSet ,
		[FromRoute] long contactId ,
		CancellationToken cancellationToken )
	{
		await RemoveContactAsync ( contactId );

		await contactSet.TryCommitAsync ( cancellationToken );

		return Results.NoContent ();

		Task RemoveContactAsync ( long contactId )
			=> contactSet.Contacts
				.Where ( contact => contact.Id == contactId )
				.ExecuteDeleteAsync ( cancellationToken );
	}
}