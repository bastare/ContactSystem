namespace ContactSystem.Core.Api.Filters;

using Core.Common.Classes.HttpMessages.Error;
using Domain.Core;

public sealed class ValidationFilter : IEndpointFilter
{
	public async ValueTask<object?> InvokeAsync ( EndpointFilterInvocationContext context , EndpointFilterDelegate next )
	{
		if ( TryResolveValidationErrors ( out var validationErrors_ , context.Arguments ) )
			return Results.BadRequest (
				error: new ErrorMessage (
					Message: string.Join (
						separator: ' ' ,
						values: validationErrors_ ) ) );

		return await next ( context );

		static bool TryResolveValidationErrors ( out ImmutableList<string> validationErrors_ , IList<object?> arguments )
		{
			arguments ??= [];

			validationErrors_ =
				arguments
					.Where ( entity => entity is IHasValidation )
					.Cast<IHasValidation> ()
					.Aggregate (
						seed: new Stack<IEnumerable<string>> () ,
						( errorMessages , entityForValidation ) =>
						{
							var validationResult_ =
								entityForValidation.Validate ();

							if ( validationResult_.IsValid )
								return errorMessages;

							errorMessages.Push ( validationResult_.Errors.Select ( error => error.ErrorMessage ) );
							return errorMessages;
						} )
					.SelectMany ( errorMessages => errorMessages )
					.ToImmutableList ();

			return validationErrors_ is { Count: > 0 };
		}
	}
}