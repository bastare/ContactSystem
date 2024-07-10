namespace ContactSystem.Core.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record PageErrorMessageWrap (
	ImmutableList<InnerErrorMessage> ErrorMessages ,
	int? StatusCode = StatusCodes.Status500InternalServerError ,
	string? Message = default ,
	string? Description = default ) :
		IHasErrorDescription,
		IHasErrorStatusCode,
		IHasErrorPage,
		IHasWrapErrorMessages
{
	public bool IsErrorPage => true;
}