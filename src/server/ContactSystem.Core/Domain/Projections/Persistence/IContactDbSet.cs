namespace ContactSystem.Core.Domain.Projections.Persistence;

using Transactions;
using Microsoft.EntityFrameworkCore;
using ContactSystem.Core.Domain.Core.Models;

public interface IContactSet : ITransaction
{
	DbSet<Contact> Contacts { get; }
}