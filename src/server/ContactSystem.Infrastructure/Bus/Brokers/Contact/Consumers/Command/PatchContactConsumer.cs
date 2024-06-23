namespace ContactSystem.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using Mapster;
using System.Threading.Tasks;
using Domain.Contracts;
using Infrastructure.Persistence.Uow.Interfaces;
using Infrastructure.Persistence.Context;
using Domain.Core.Models.Contact;
using Domain.Contracts.ContactContracts.Command.PatchContact.Dtos;
using Domain.Contracts.ContactContracts.Command.PatchContact;
using Domain.Shared.Common.Exceptions;

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
				new ( ContactFromPatch: patchedContact_.Adapt<ContactFromPatchDto> () ) );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		async Task<Contact> PatchContactAsync ( ContactForPatchDto contactForPatch )
		{
			// TODO: Integrate `ExecuteUpdate`
			var config = new TypeAdapterConfig ();
			config.Default.IgnoreNullValues ( true );

			var contactForUpdate_ =
				await _efUnitOfWork.Repository<Contact> ()
					.FindByAsync (
						contact => contact.Id == contactForPatch.Id ,
						isTracking: true ,
						cancellationToken: context.CancellationToken ) ??
							throw new NotFoundException ( message: $"There is no `Contact` with this id: {contactForPatch.Id}" );

			return contactForPatch.Adapt ( contactForUpdate_ , config )!;
		}
	}
}