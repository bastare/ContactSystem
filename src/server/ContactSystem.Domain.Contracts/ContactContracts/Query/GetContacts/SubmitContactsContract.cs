namespace ContactSystem.Domain.Contracts.ContactContracts.Query.GetContacts;

using Dtos;
using Domain.Contracts.Dtos.WrapDtos.Interfaces;

public sealed record SubmitContactsContract ( IPaginationRowsDto ContactsForQueryResponse );