namespace ContactSystem.Core.Infrastructure.GlobalExceptionHandler;

using Common.Classes.HttpMessages.Error;
using Common.Constants;
using ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;

public sealed class ExceptionHandlerManager
{
	private const string JsonErrorMediaType = "application/problem+json";

	private readonly ImmutableList<IExceptionHandler> _exceptionHandlers;

	private readonly IExceptionHandler _defaultExceptionHandler;

	internal ExceptionHandlerManager (
		ImmutableList<IExceptionHandler> exceptionHandlers ,
		IExceptionHandler defaultExceptionHandler )
	{
		ExceptionHandlersAreUniq ( exceptionHandlers );

		_exceptionHandlers = exceptionHandlers;
		_defaultExceptionHandler = defaultExceptionHandler;

		static void ExceptionHandlersAreUniq ( IEnumerable<IExceptionHandler> exceptionHandlers )
		{
			if ( !AreUnique ( exceptionHandlers ) )
				throw new ArgumentException ( "There are 1 or more error handler(-s), that have duplicated `id`" );

			static bool AreUnique ( IEnumerable<IExceptionHandler> exceptionHandlers )
				=> exceptionHandlers.DistinctBy ( exceptionHandler => exceptionHandler.Id ).Count ()
					== exceptionHandlers.Count ();
		}
	}

	public async Task FormErrorResponseAsync ( HttpContext? httpContext , Exception? exception )
	{
		try
		{
			ArgumentNullException.ThrowIfNull ( httpContext );
			ArgumentNullException.ThrowIfNull ( exception );

			InjectJsonErrorMediaType ( httpContext! );

			if ( TryHoldException ( out IExceptionHandler? exceptionHandler_ , httpContext! , exception! ) )
			{
				await FormExceptionHandlerErrorResponseAsync (
					exceptionHandler_! ,
					httpContext! ,
					exception! ,
					cancellationToken: httpContext!.RequestAborted );

				return;
			}

			await FormUnexpectableHandlerErrorResponseAsync (
				httpContext! ,
				exception! ,
				cancellationToken: httpContext!.RequestAborted );
		}
		catch ( Exception )
		{
			await FormInnerErrorResponseAsync (
				httpContext! ,
				cancellationToken: httpContext!.RequestAborted );
		}

		static void InjectJsonErrorMediaType ( HttpContext httpContext )
		{
			httpContext.Response.ContentType = JsonErrorMediaType;
		}

		bool TryHoldException ( out IExceptionHandler? exceptionHandler_ , HttpContext httpContext , Exception exception )
		{
			exceptionHandler_ = ResolveExceptionHandlersThatHoldRaisedException ( httpContext , exception );

			return exceptionHandler_ is not null;

			IExceptionHandler? ResolveExceptionHandlersThatHoldRaisedException ( HttpContext httpContext , Exception exception )
			{
				return ResolveSingleExceptionHandler (
					exceptionHandlers: ResolveExceptionHandlersThatHoldRaisedException ( httpContext , exception ) ,
					exception );

				IEnumerable<IExceptionHandler> ResolveExceptionHandlersThatHoldRaisedException ( HttpContext httpContext , Exception exception )
					=> _exceptionHandlers
						.Where ( exceptionHandler =>
							exceptionHandler.IsHold ( httpContext , exception ) );

				static IExceptionHandler? ResolveSingleExceptionHandler ( IEnumerable<IExceptionHandler> exceptionHandlers , Exception exception )
					=> exceptionHandlers.Count () switch
					{
						1 => exceptionHandlers.First (),
						0 => default,
						> 1 => throw new ArgumentException ( message: CreateErrorMessage ( exceptionHandlers , exception ) , nameof ( exceptionHandlers ) ),
						_ => throw new ArgumentException ( message: $"No case for this condition: {exceptionHandlers.Count ()}" , nameof ( exceptionHandlers ) )
					};

				static string CreateErrorMessage ( IEnumerable<IExceptionHandler> exceptionHandlers , Exception exception )
					=> new StringBuilder ()
						.Append ( "There are collision between 2 or more exception handlers, on " )
						.Append ( exception.GetType ().ShortDisplayName () )
						.Append ( ", between: " )
						.AppendJoin (
							separator: ", " ,
							exceptionHandlers.Select ( exceptionHandler => exceptionHandler.Id ) )
						.Append ( " - error handler" )

						.ToString ();
			}
		}

		Task FormUnexpectableHandlerErrorResponseAsync ( HttpContext httpContext , Exception exception , CancellationToken cancellationToken = default )
			=> FormExceptionHandlerErrorResponseAsync (
				exceptionHandler: _defaultExceptionHandler ,
				httpContext ,
				exception ,
				cancellationToken );

		static async Task FormExceptionHandlerErrorResponseAsync ( IExceptionHandler exceptionHandler ,
																   HttpContext httpContext ,
																   Exception exception ,
																   CancellationToken cancellationToken = default )
		{
			InvokeStatusCode ( exceptionHandler , httpContext , exception );

			await EmitExceptionHandlerCallbackAsync ( exceptionHandler , httpContext , exception );

			await FormErrorMessageAsync ( exceptionHandler , httpContext , exception );

			static void InvokeStatusCode ( IExceptionHandler exceptionHandler , HttpContext httpContext , Exception exception )
			{
				httpContext!.Response.StatusCode = ( int ) exceptionHandler.InjectStatusCode.Invoke ( httpContext , exception );
			}

			Task EmitExceptionHandlerCallbackAsync ( IExceptionHandler exceptionHandler , HttpContext httpContext , Exception exception )
				=> exceptionHandler.OnHoldAsync?.Invoke ( httpContext , exception , cancellationToken ) ??
					Task.CompletedTask;

			Task FormErrorMessageAsync ( IExceptionHandler exceptionHandler ,
										 HttpContext httpContext ,
										 Exception exception )
			{
				return httpContext.Response.WriteAsync (
					text: JsonConvert.SerializeObject (
						value: ResolveExceptionMessage ( exceptionHandler , exception ) ,
						JsonConversationSettings.DefaultSerializerSettings ) ,
					cancellationToken );

				static object ResolveExceptionMessage ( IExceptionHandler exceptionHandler , Exception exception )
					=> exceptionHandler.InjectExceptionMessage.Invoke ( exception );
			}
		}

		static Task FormInnerErrorResponseAsync ( HttpContext httpContext ,
												  CancellationToken cancellationToken = default )
		{
			httpContext!.Response.StatusCode = StatusCodes.Status500InternalServerError;

			return httpContext.Response.WriteAsync (
				text: JsonConvert.SerializeObject (
					value: new ErrorMessage (
						Message: "Internal server error" ,
						StatusCode: StatusCodes.Status500InternalServerError ) ,
					JsonConversationSettings.DefaultSerializerSettings ) ,
				cancellationToken );
		}
	}
}