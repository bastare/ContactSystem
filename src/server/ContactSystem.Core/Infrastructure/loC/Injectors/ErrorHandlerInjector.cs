namespace ContactSystem.Core.Infrastructure.loC.Injectors;

using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Common.Classes.HttpMessages.Error;
using Common.Exceptions;
using GlobalExceptionHandler;
using GlobalExceptionHandler.Builders;
using GlobalExceptionHandler.ExceptionHandlers;
using Interfaces;
using Domain.Validation.Common.Exceptions;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class ErrorHandlerInjector : IInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		serviceCollection.TryAddSingleton ( instance: CreateGlobalExceptionHandlerManager () );

		static ExceptionHandlerManager CreateGlobalExceptionHandlerManager ()
			=> ExceptionHandlerManagerBuilder.Create ()
				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 1 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( FormatException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.BadRequest ,
						InjectExceptionMessage = ( _ ) =>
							new PageErrorMessage (
								StatusCode: StatusCodes.Status400BadRequest ,
								Message: "Unexpected format" ,
								Description: "Sorry, try use other format." )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 2 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( ForbiddenException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.Forbidden ,
						InjectExceptionMessage = ( _ ) =>
							new PageErrorMessage (
								StatusCode: StatusCodes.Status403Forbidden ,
								Message: "Forbidden" ,
								Description: "User have no permission to this resource" )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 3 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( NotFoundException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.NotFound ,
						InjectExceptionMessage = ( exception ) =>
							new PageErrorMessage (
								StatusCode: StatusCodes.Status404NotFound ,
								Message: exception.Message )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 4 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( HttpRequestException ) )
					{
						InjectStatusCode = ( httpContext , _ ) =>
							httpContext.ResolveException<HttpRequestException> ()!.StatusCode!.Value ,
						InjectExceptionMessage = ( exception ) =>
							new PageErrorMessage (
								StatusCode: ( int ) ( ( HttpRequestException ) exception ).StatusCode!.Value ,
								Message: exception.Message )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 5 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( ArgumentException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.BadRequest ,
						InjectExceptionMessage = ( exception ) =>
							new PageErrorMessage (
								StatusCode: ( int ) HttpStatusCode.BadRequest ,
								Message: exception.Message )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 6 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( DbUpdateException )
								&& exception.InnerException!.Message.Contains ( "UNIQUE constraint failed: Contacts.Email" ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.BadRequest ,
						InjectExceptionMessage = ( exception ) =>
							new PageErrorMessage (
								StatusCode: StatusCodes.Status400BadRequest ,
								Message: "The user with this email created already" )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 7 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( ValidationFailureException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.BadRequest ,
						InjectExceptionMessage = ( exception ) =>
							new ErrorMessage (
								Message: string.Join (
									separator: ' ' ,
									values: ( ( ValidationFailureException ) exception )
										.FailedValidationResult
										.Errors
											.Select ( validationFailure => validationFailure.ErrorMessage ) ) )
					} )

				.Build ();
	}
}