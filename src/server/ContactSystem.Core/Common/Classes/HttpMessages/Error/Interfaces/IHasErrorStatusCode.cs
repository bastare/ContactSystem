namespace ContactSystem.Core.Common.Classes.HttpMessages.Error.Interfaces;

public interface IHasErrorStatusCode
{
	public int? StatusCode { get; }
}