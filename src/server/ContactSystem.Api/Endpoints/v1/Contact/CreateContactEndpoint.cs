namespace ContactSystem.Api.Endpoints.v1.Contact;

using Contracts;
using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Command.CreateContact.Dtos;
using Domain.Contracts.ContactContracts.Command.CreateContact;
using Mapster;

public sealed class CreateContactsEndpoint ( IRequestClient<CreateContactContract> requestClient )
    : Endpoint<ContactForCreationRequestBody>
{
    private readonly IRequestClient<CreateContactContract> _requestClient = requestClient;

    public override void Configure ()
    {
        Verbs ( Http.POST );
        Routes ( "api/v1/contacts" );
		AllowAnonymous ();
    }

    public override async Task HandleAsync ( ContactForCreationRequestBody requestBody , CancellationToken cancellationToken = default )
    {
        var (response, fault) =
            await _requestClient.GetResponse<SubmitCreatedContactContract , FaultContract> (
                new ( requestBody.Adapt<ContactForCreationDto>() ) ,
                cancellationToken );

        if ( !response.IsCompletedSuccessfully )
            throw ( await fault ).Message.Exception;

        await SendAsync (
            response: (await response).Message.CreatedContact ,
            cancellation: cancellationToken );
    }
}