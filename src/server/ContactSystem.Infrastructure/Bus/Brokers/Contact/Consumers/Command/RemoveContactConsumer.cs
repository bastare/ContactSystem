namespace ContactSystem.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using System.Threading.Tasks;
using Domain.Contracts;
using ContactSystem.Infrastructure.Persistence.Uow.Interfaces;
using ContactSystem.Infrastructure.Persistence.Context;
using Domain.Core.Models.Contact;
using Domain.Contracts.ContactContracts.Command.RemoveContact;

public sealed class RemoveContactConsumer ( IEfUnitOfWork<EfContext , int> efUnitOfWork ) :
	IConsumer<RemoveContactContract>
{
	private readonly IEfUnitOfWork<EfContext , int> _efUnitOfWork = efUnitOfWork;


	public async Task Consume ( ConsumeContext<RemoveContactContract> context )
	{
		try
		{
			await RemoveContactsAsync ( context.Message.Id );

			await _efUnitOfWork.TryCommitAsync ( context.CancellationToken );

			await context.RespondAsync<SubmitRemovedContactsContract> ( new () );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		Task RemoveContactsAsync ( int contactIdForRemove )
			=> _efUnitOfWork.Repository<Contact> ()
				.RemoveByAsync (
					( contact ) => contact.Id == contactIdForRemove ,
					cancellationToken: context.CancellationToken );
	}
}