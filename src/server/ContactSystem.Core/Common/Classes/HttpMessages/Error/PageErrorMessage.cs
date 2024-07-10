namespace ContactSystem.Core.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record PageErrorMessage (
	int? StatusCode = StatusCodes.Status500InternalServerError ,
	string? Message = default ,
	string? Description = default ) :
		IHasErrorDescription,
		IHasErrorStatusCode,
		IHasErrorPage
{
	public bool IsErrorPage => true;
}