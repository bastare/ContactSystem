namespace ContactSystem.Api.Endpoints.v1.Contact;

using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Command.PatchContact.Dtos;
using Domain.Contracts.ContactContracts.Command.PatchContact;
using Mapster;
using ContactSystem.Api.Endpoints.v1.Contact.Contracts;

public sealed class PatchContactEndpoint ( IRequestClient<PatchContactContract> getHomeRequestClient )
	: Endpoint<ContactForPatchRequestBody>
{
	private readonly IRequestClient<PatchContactContract> _getHomeRequestClient = getHomeRequestClient;

	public override void Configure ()
	{
		Verbs ( Http.PATCH );
		Routes ( "api/v1/contacts" );
		AllowAnonymous ();
	}

	public override async Task HandleAsync ( ContactForPatchRequestBody requestBody , CancellationToken cancellationToken = default )
	{
		var (response, fault) =
			await _getHomeRequestClient.GetResponse<SubmitPatchedContactsContract , FaultContract> (
				new ( requestBody.Adapt<ContactForPatchDto>() ) ,
				cancellationToken );

		if ( !response.IsCompletedSuccessfully )
			throw ( await fault ).Message.Exception;

		await SendAsync (
			response: ( await response ).Message.ContactFromPatch ,
			cancellation: cancellationToken );
	}
}