namespace ContactSystem.Core.Common.Classes.HttpMessages.Error.Interfaces;

public interface IHasErrorDescription
{
	public string? Message { get; }

	public string? Description { get; }
}