namespace ContactSystem.Core.Domain.Projections.Persistence;

using Transactions;
using Microsoft.EntityFrameworkCore;

public interface IContactSet : ITransaction
{
	DbSet<Contact> Contacts { get; }
}