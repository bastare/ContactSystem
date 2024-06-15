namespace ContactSystem.Domain.Shared.Common.Classes.HttpMessages.Error.Interfaces;

public interface IHasWrapErrorMessages
{
	ImmutableList<InnerErrorMessage> ErrorMessages { get; }
}