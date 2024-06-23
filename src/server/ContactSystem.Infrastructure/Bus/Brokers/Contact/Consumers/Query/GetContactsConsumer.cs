namespace ContactSystem.Infrastructure.Bus.Brokers.Contact.Consumers.Query;

using MassTransit;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Query.GetContacts;
using Domain.Core.Models.Contact;
using Domain.Contracts.Dtos.WrapDtos.Interfaces;
using Persistence.Uow.Interfaces;
using Persistence.Context;
using Persistence.Pagination.Interfaces;
using Persistence.Specifications.Inline;
using Mapster;

public sealed class GetContactsConsumer ( IEfUnitOfWork<EfContext , int> efUnitOfWork ) :
	IConsumer<GetContactsContract>
{
	private readonly IEfUnitOfWork<EfContext , int> _efUnitOfWork = efUnitOfWork;

	public async Task Consume ( ConsumeContext<GetContactsContract> context )
	{
		try
		{
			var contacts_ = await GetContactsAsync ( context.Message );

			await context.RespondAsync<SubmitContactsContract> (
				new ( ContactsForQueryResponse: contacts_.Adapt<IPaginationRowsDto> () ) );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		async Task<IPagedList> GetContactsAsync ( GetContactsContract getContactsContract )
			=> await _efUnitOfWork.Repository<Contact> ()
				.FilterByAsync (
					specification: new InlinePaginationQuerySpecification<Contact , int> (
						getContactsContract.ExpressionQuery ,
						getContactsContract.OrderQuery ,
						getContactsContract.PaginationQuery ,
						getContactsContract.ProjectionQuery ) ,
					context.CancellationToken );
	}
}