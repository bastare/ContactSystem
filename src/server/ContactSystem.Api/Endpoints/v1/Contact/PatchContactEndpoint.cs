namespace ContactSystem.Api.Endpoints.v1.Contact;

using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Command.PatchContact.Dtos;
using Domain.Contracts.ContactContracts.Command.PatchContact;

public sealed class PatchContactEndpoint ( IRequestClient<PatchContactContract> getHomeRequestClient )
	: Endpoint<ContactForPatchDto>
{
	private readonly IRequestClient<PatchContactContract> _getHomeRequestClient = getHomeRequestClient;

	public override void Configure ()
	{
		Verbs ( Http.PATCH );
		Routes ( "api/v1/contacts" );
		AllowAnonymous ();
	}

	public override async Task HandleAsync ( ContactForPatchDto requestBody , CancellationToken cancellationToken = default )
	{
		var (response, fault) =
			await _getHomeRequestClient.GetResponse<SubmitPatchedContactsContract , FaultContract> (
				new ( requestBody ) ,
				cancellationToken );

		if ( !response.IsCompletedSuccessfully )
			throw ( await fault ).Message.Exception;

		await SendAsync (
			response: ( await response ).Message.ContactFromPatch ,
			cancellation: cancellationToken );
	}
}