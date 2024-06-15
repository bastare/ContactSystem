namespace ContactSystem.Api.Endpoints.v1.Contact;

using FastEndpoints;
using MassTransit;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Query.GetContacts;
using Mapster;
using ContactSystem.Domain.Contracts.Dtos.QueryDtos;
using Queries.Interfaces;
using static GetContactsEndpoint;

public sealed class GetContactsEndpoint ( IRequestClient<GetContactsContract> getHomeRequestClient )
	: Endpoint<GetContactsQuery>
{
	public sealed record GetContactsQuery :
		IExpressionQuery,
		IOrderQuery,
		IPaginationQuery,
		IProjectionQuery
	{
		public string? Expression { get; init; }

		public string? Projection { get; init; }

		public int Offset { get; init; }

		public int Limit { get; init; }

		public bool IsDescending { get; init; }

		public string? OrderBy { get; init; }
	}

	private readonly IRequestClient<GetContactsContract> _getHomeRequestClient = getHomeRequestClient;

	public override void Configure ()
	{
		Verbs ( Http.GET );
		Routes ( "api/v1/contacts" );
		AllowAnonymous ();
	}

	public override async Task HandleAsync ( GetContactsQuery requestQuery , CancellationToken cancellationToken = default )
	{
		var (response, fault) =
			await _getHomeRequestClient.GetResponse<SubmitContactsContract , FaultContract> (
				new ( requestQuery?.Adapt<ExpressionQueryDto> () ,
					  requestQuery?.Adapt<OrderQueryDto> () ,
					  requestQuery?.Adapt<PaginationQueryDto> () ,
					  requestQuery?.Adapt<ProjectionQueryDto> () ) ,
				cancellationToken );

		if ( !response.IsCompletedSuccessfully )
			throw ( await fault ).Message.Exception;

		await SendAsync (
			response: (await response).Message ,
			cancellation: cancellationToken );
	}
}