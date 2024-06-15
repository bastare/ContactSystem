namespace ContactSystem.Domain.Contracts.ContactContracts.Command.PatchContact;

using Dtos;

public sealed record SubmitPatchedContactsContract ( ContactFromPatchDto ContactFromPatch );