namespace ContactSystem.Api.Endpoints.v1.Contact;

using Contracts;
using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Command.PatchContact;

public sealed class PatchContactEndpoint ( IRequestClient<PatchContactContract> requestClient )
	: Endpoint<ContactForPatchRequestBody>
{
	private readonly IRequestClient<PatchContactContract> _requestClient = requestClient;

	public override void Configure ()
	{
		Verbs ( Http.PATCH );
		Routes ( "api/v1/contacts/{id}" );
		AllowAnonymous ();
	}

	public override async Task HandleAsync ( ContactForPatchRequestBody requestBody , CancellationToken cancellationToken = default )
	{
		var (response, fault) =
			await _requestClient.GetResponse<SubmitPatchedContactsContract , FaultContract> (
				new ( ContactForPatch: new ()
				{
					Id = ResolveIdFromQuery () ,
					FirstName = requestBody.FirstName ,
					LastName = requestBody.LastName ,
					Email = requestBody.Email ,
					Phone = requestBody.Phone ,
					Title = requestBody.Title ,
					MiddleInitial = requestBody.MiddleInitial
				} ) ,
				cancellationToken );

		if ( !response.IsCompletedSuccessfully )
			throw ( await fault ).Message.Exception;

		await SendAsync (
			response: ( await response ).Message.ContactFromPatch ,
			cancellation: cancellationToken );

		int ResolveIdFromQuery ()
		{
			HttpContext.Request.RouteValues.TryGetValue ( "id" , out var idRouteFragment );

			if ( string.IsNullOrEmpty ( idRouteFragment?.ToString () ) )
				throw new ArgumentException ( $"There is no `Id` fragment" );

			return int.TryParse ( idRouteFragment?.ToString () , out var id )
				? id
				: throw new ArgumentException ( $"`Id` value with wrong type: {id.GetType ().Name}" );
		}
	}
}