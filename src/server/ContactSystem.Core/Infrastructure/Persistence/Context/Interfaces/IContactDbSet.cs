namespace ContactSystem.Core.Infrastructure.Persistence.Context.Interfaces;

using Domain.Contact;
using Transactions;
using Microsoft.EntityFrameworkCore;

public interface IContactSet : ITransaction
{
	DbSet<Contact> Contacts { get; }
}