namespace ContactSystem.Core.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record InnerErrorMessage (
	string? Message = default ,
	int? StatusCode = default ,
	string? Description = default ) :
		IHasErrorStatusCode, IHasErrorDescription;