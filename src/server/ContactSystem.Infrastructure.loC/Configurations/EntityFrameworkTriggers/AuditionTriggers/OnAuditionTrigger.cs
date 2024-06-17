namespace ContactSystem.Infrastructure.loC.Configurations.EntityFrameworkTriggers.AuditionTriggers;

using Domain.Core.Models;
using Domain.Shared.Authorization.Session.Interfaces;
using EntityFrameworkCore.Triggered;

public sealed class OnAuditionTrigger ( IUserSession session ) : IBeforeSaveTrigger<IAuditable<int>>
{
	private readonly IUserSession _session = session;

	public Task BeforeSave ( ITriggerContext<IAuditable<int>> context , CancellationToken cancellationToken )
	{
		return _session.IsAuthorizedUser ()
			? InsertUserAuditionData ()
			: InsertAuditionData ();

		Task InsertUserAuditionData ()
		{
			switch ( context.ChangeType )
			{
				case ChangeType.Added:
					context.Entity.CreatedBy = _session.Id!.Value;
					context.Entity.Created = DateTime.UtcNow;

					break;

				case ChangeType.Modified:
					context.Entity.LastModifiedBy = _session.Id!.Value;
					context.Entity.LastModified = DateTime.UtcNow;

					break;
			}

			return Task.CompletedTask;
		}

		Task InsertAuditionData ()
		{
			switch ( context.ChangeType )
			{
				case ChangeType.Added:
					context.Entity.Created = DateTime.UtcNow;

					break;

				case ChangeType.Modified:
					context.Entity.LastModified = DateTime.UtcNow;

					break;
			}

			return Task.CompletedTask;
		}
	}
}