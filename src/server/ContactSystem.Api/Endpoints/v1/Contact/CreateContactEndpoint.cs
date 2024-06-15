namespace ContactSystem.Api.Endpoints.v1.Contact;

using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using ContactSystem.Domain.Contracts.ContactContracts.Command.CreateContact.Dtos;
using ContactSystem.Domain.Contracts.ContactContracts.Command.CreateContact;

public sealed class CreateContactsEndpoint ( IRequestClient<CreateContactContract> getHomeRequestClient )
    : Endpoint<ContactForCreationDto>
{
    private readonly IRequestClient<CreateContactContract> _getHomeRequestClient = getHomeRequestClient;

    public override void Configure ()
    {
        Verbs ( Http.POST );
        Routes ( "api/v1/contacts" );
		AllowAnonymous ();
    }

    public override async Task HandleAsync ( ContactForCreationDto requestBody , CancellationToken cancellationToken = default )
    {
        var (response, fault) =
            await _getHomeRequestClient.GetResponse<SubmitCreatedContactContract , FaultContract> (
                new ( requestBody ) ,
                cancellationToken );

        if ( !response.IsCompletedSuccessfully )
            throw ( await fault ).Message.Exception;

        await SendAsync (
            response: (await response).Message.CreatedContact ,
            cancellation: cancellationToken );
    }
}