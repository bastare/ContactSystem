namespace ContactSystem.Core.Api.Filters;

using Core.Common.Classes.HttpMessages.Error;
using Domain.Core;
using System.Threading.Tasks;

public sealed class ValidationFilter : IEndpointFilter
{
	public async ValueTask<object?> InvokeAsync ( EndpointFilterInvocationContext context , EndpointFilterDelegate next )
	{
		var validationErrors =
			context.Arguments
				.Where ( entity => entity is IHasValidation )
				.Cast<IHasValidation> ()
				.Aggregate (
					seed: new Stack<IEnumerable<string>> () ,
					( errorMessages , entityForValidation ) =>
					{
						var validationResult = entityForValidation.Validate ();

						if ( validationResult.IsValid )
							return errorMessages;

						errorMessages.Push ( validationResult.Errors.Select ( error => error.ErrorMessage ) );
						return errorMessages;
					} )
				.SelectMany ( errorMessages => errorMessages )
				.ToImmutableList ();

		if ( validationErrors is { Count: <= 0 } )
			return await next ( context );

		return Results.BadRequest (
			error: new ErrorMessage (
				Message: string.Join (
					separator: ' ' ,
					values: validationErrors ) ) );
	}
}