namespace ContactSystem.Core.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record PageErrorMessageWrap (
	ImmutableList<InnerErrorMessage> ErrorMessages ,
	int? StatusCode = 500 ,
	string? Message = default ,
	string? Description = default ) :
		IHasErrorDescription,
		IHasErrorStatusCode,
		IHasErrorPage,
		IHasWrapErrorMessages
{
	public bool IsErrorPage => true;
}