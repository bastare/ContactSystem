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

public sealed class CreateContactConsumer ( IEfUnitOfWork<EfContext , int> efUnitOfWork ) :
	IConsumer<CreateContactContract>
{
	private readonly IEfUnitOfWork<EfContext , int> _efUnitOfWork = efUnitOfWork;

	public async Task Consume ( ConsumeContext<CreateContactContract> context )
	{
		try
		{
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
	}
}