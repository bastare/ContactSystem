namespace ContactSystem.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using Mapster;
using System.Threading.Tasks;
using Domain.Contracts;
using ContactSystem.Infrastructure.Persistence.Uow.Interfaces;
using ContactSystem.Infrastructure.Persistence.Context;
using ContactSystem.Domain.Core.Models.Contact;
using ContactSystem.Domain.Contracts.ContactContracts.Command.CreateContact.Dtos;
using ContactSystem.Domain.Contracts.ContactContracts.Command.CreateContact;
using ContactSystem.Domain.Caching.Interfaces;

public sealed class CreateContactConsumer ( IEfUnitOfWork<EfContext , int> efUnitOfWork , ICacheService cacheService ) :
	IConsumer<CreateContactContract>
{
	private readonly IEfUnitOfWork<EfContext , int> _efUnitOfWork = efUnitOfWork;

	private readonly ICacheService _cacheService = cacheService;

	public async Task Consume ( ConsumeContext<CreateContactContract> context )
	{
		try
		{
			// ? Indexer doesn't work with `InMemory` :(
			await FeatureNotBugEmailIndexerAsync ( context.Message.ContactForCreation );

			var createdContact_ =
				await CreateContactsAsync ( context.Message.ContactForCreation );

			await _efUnitOfWork.SaveChangesAsync ( context.CancellationToken );

			await context.RespondAsync<SubmitCreatedContactContract> (
				new ( CreatedContact: createdContact_.Adapt<ContactFromCreationDto> () ) );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		Task<Contact> CreateContactsAsync ( ContactForCreationDto contactForCreation )
			=> _efUnitOfWork.Repository<Contact> ()
				.AddAsync (
					contactForCreation.Adapt<Contact> () ,
					context.CancellationToken );

		Task FeatureNotBugEmailIndexerAsync ( ContactForCreationDto contactForCreation )
		{
			var domainKey = $"[FeatureNotBugEmailIndexer]:{contactForCreation.Email}";

			if ( _cacheService.TryGet<string> ( domainKey , out var email ) )
				throw new ArgumentException ( $"There is 1 contact with this email address: {email}" );

			return _cacheService.SetAsync (
				key: domainKey ,
				value: contactForCreation.Email! ,
				expireSpan: TimeSpan.MaxValue ,
				context.CancellationToken );
		}
	}
}