namespace ContactSystem.Api.Endpoints.v1.Contact;

using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Command.RemoveContact;
using static RemoveContactEndpoint;

public sealed class RemoveContactEndpoint ( IRequestClient<RemoveContactContract> getHomeRequestClient )
    : Endpoint<RemoveContactRoute>
{
	public sealed record RemoveContactRoute
	{
		public int Id { get; init; }
	}

    private readonly IRequestClient<RemoveContactContract> _getHomeRequestClient = getHomeRequestClient;

    public override void Configure ()
    {
        Verbs ( Http.DELETE );
        Routes ( "api/v1/contacts/{id}" );
		AllowAnonymous ();
    }

    public override async Task HandleAsync ( RemoveContactRoute removeContactRoute , CancellationToken cancellationToken = default )
    {
        var (response, fault) =
            await _getHomeRequestClient.GetResponse<SubmitRemovedContactsContract , FaultContract> (
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