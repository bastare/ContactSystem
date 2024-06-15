namespace ContactSystem.Domain.Contracts.ContactContracts.Command.CreateContact;

using Common.Attributes;
using Dtos;

[RequestClientContract]
public sealed record CreateContactContract ( ContactForCreationDto ContactForCreation );