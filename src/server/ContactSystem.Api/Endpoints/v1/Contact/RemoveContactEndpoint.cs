namespace ContactSystem.Api.Endpoints.v1.Contact;

using Contracts;
using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Command.RemoveContact;

public sealed class RemoveContactEndpoint ( IRequestClient<RemoveContactContract> requestClient )
    : Endpoint<RemoveContactRoute>
{
    private readonly IRequestClient<RemoveContactContract> _requestClient = requestClient;

    public override void Configure ()
    {
        Verbs ( Http.DELETE );
        Routes ( "api/v1/contacts/{id}" );
		AllowAnonymous ();
    }

    public override async Task HandleAsync ( RemoveContactRoute removeContactRoute , CancellationToken cancellationToken = default )
    {
        var (response, fault) =
            await _requestClient.GetResponse<SubmitRemovedContactsContract , FaultContract> (
                new ( removeContactRoute.Id ) ,
                cancellationToken );

        if ( !response.IsCompletedSuccessfully )
            throw ( await fault ).Message.Exception;

        await SendAsync (
            response: null ,
            statusCode: StatusCodes.Status204NoContent ,
            cancellation: cancellationToken );
    }
}