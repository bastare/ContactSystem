namespace ContactSystem.Core.Domain.Projections.Persistence;

using Transactions;
using Microsoft.EntityFrameworkCore;
using Core.Models;

public interface IContactSet : ITransaction
{
	DbSet<Contact> Contacts { get; }
}