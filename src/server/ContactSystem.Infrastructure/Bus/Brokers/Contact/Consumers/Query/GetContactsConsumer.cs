namespace ContactSystem.Infrastructure.Bus.Brokers.Contact.Consumers.Query;

using MassTransit;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Query.GetContacts;
using ContactSystem.Infrastructure.Persistence.Uow.Interfaces;
using ContactSystem.Infrastructure.Persistence.Context;
using ContactSystem.Infrastructure.Persistence.Specifications.Inline;
using ContactSystem.Domain.Core.Models.Contact;
using ContactSystem.Infrastructure.Persistence.Pagination.Interfaces;
using ContactSystem.Domain.Contracts.Dtos.WrapDtos.Interfaces;
using Mapster;

public sealed class GetContactsConsumer ( IEfUnitOfWork<EfContext , int> efUnitOfWork ) :
	IConsumer<GetContactsContract>
{
	private readonly IEfUnitOfWork<EfContext , int> _efUnitOfWork = efUnitOfWork;

	public async Task Consume ( ConsumeContext<GetContactsContract> context )
	{
		try
		{
			var contacts_ = await GetContactsAsync ();

			await context.RespondAsync<SubmitContactsContract> (
				new ( ContactsForQueryResponse: contacts_.Adapt<IPaginationRowsDto> ()  ) );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		async Task<IPagedList> GetContactsAsync ()
			=> await _efUnitOfWork.Repository<Contact> ()
				.FilterByAsync (
					specification: new InlinePaginationQuerySpecification<Contact , int> (
						context.Message.ExpressionQuery ,
						context.Message.OrderQuery ,
						context.Message.PaginationQuery ,
						context.Message.ProjectionQuery ) );
	}
}