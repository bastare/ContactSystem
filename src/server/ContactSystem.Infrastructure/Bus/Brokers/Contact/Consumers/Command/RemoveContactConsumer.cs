namespace ContactSystem.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using System.Threading.Tasks;
using Domain.Contracts;
using ContactSystem.Infrastructure.Persistence.Uow.Interfaces;
using ContactSystem.Infrastructure.Persistence.Context;
using Domain.Core.Models.Contact;
using Domain.Contracts.ContactContracts.Command.RemoveContact;
using ContactSystem.Domain.Caching.Interfaces;

public sealed class RemoveContactConsumer ( IEfUnitOfWork<EfContext , int> efUnitOfWork , ICacheService cacheService ) :
	IConsumer<RemoveContactContract>
{
	private readonly IEfUnitOfWork<EfContext , int> _efUnitOfWork = efUnitOfWork;

	private readonly ICacheService _cacheService = cacheService;

	public async Task Consume ( ConsumeContext<RemoveContactContract> context )
	{
		try
		{
			var removedContact =
				await RemoveContactsAsync ( context.Message.Id );

			if ( removedContact is not null )
				// ? Indexer doesn't work with `InMemory` :(
				FeatureNotBugEmailIndexer ( contactEmailForCacheRemove: removedContact.Email! );

			await _efUnitOfWork.TryCommitAsync ( context.CancellationToken );

			await context.RespondAsync<SubmitRemovedContactsContract> ( new () );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		Task<Contact?> RemoveContactsAsync ( int contactIdForRemove )
			=> _efUnitOfWork.Repository<Contact> ()
				.RemoveByAsync (
					( contact ) => contact.Id == contactIdForRemove ,
					cancellationToken: context.CancellationToken );

		void FeatureNotBugEmailIndexer ( string contactEmailForCacheRemove )
		{
			_cacheService.Remove (
				key: $"[FeatureNotBugEmailIndexer]:{contactEmailForCacheRemove}" );
		}
	}
}