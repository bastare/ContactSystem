namespace ContactSystem.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using Mapster;
using System.Threading.Tasks;
using Domain.Contracts;
using ContactSystem.Infrastructure.Persistence.Uow.Interfaces;
using ContactSystem.Infrastructure.Persistence.Context;
using ContactSystem.Domain.Core.Models.Contact;
using ContactSystem.Domain.Contracts.ContactContracts.Command.PatchContact.Dtos;
using ContactSystem.Domain.Contracts.ContactContracts.Command.PatchContact;

public sealed class PatchContactConsumer ( IEfUnitOfWork<EfContext , int> efUnitOfWork ) :
	IConsumer<PatchContactContract>
{
	private readonly IEfUnitOfWork<EfContext , int> _efUnitOfWork = efUnitOfWork;

	public async Task Consume ( ConsumeContext<PatchContactContract> context )
	{
		try
		{
			var patchedContact_ =
				await PatchContactAsync ( context.Message.ContactForPatch );

			await _efUnitOfWork.TryCommitAsync ( context.CancellationToken );

			await context.RespondAsync<SubmitPatchedContactsContract> (
				new ( ContactFromPatch: patchedContact_.Adapt<ContactFromPatchDto>() ) );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		async Task<Contact> PatchContactAsync ( ContactForPatchDto contactForPatch )
		{
			var config = new TypeAdapterConfig();
			config.Default.IgnoreNullValues(true);

			var contactForUpdate =
				await _efUnitOfWork.Repository<Contact> ()
					.FindByAsync (
						contact => contact.Id == contactForPatch.Id ,
						isTracking: true ,
						cancellationToken: context.CancellationToken );

			return await _efUnitOfWork.Repository<Contact> ()
				.UpdateAsync (
					contactForPatch.Adapt ( contactForUpdate, config )! ,
					context.CancellationToken );
		}
	}
}