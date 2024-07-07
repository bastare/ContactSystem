namespace ContactSystem.Core.loC.Injectors;

using Common.Classes.HttpMessages.Error;
using Common.Exceptions;
using Infrastructure.GlobalExceptionHandler;
using Infrastructure.GlobalExceptionHandler.Builders;
using Infrastructure.GlobalExceptionHandler.ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

public static class ErrorHandlerInjector
{
	public static void Inject ( IServiceCollection serviceCollection )
	{
		serviceCollection.AddSingleton ( implementationInstance: CreateGlobalExceptionHandlerManager () );

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
						InjectExceptionMessage = ( _ ) =>
							new PageErrorMessage (
								StatusCode: StatusCodes.Status404NotFound ,
								Message: "The requested url is not found" ,
								Description: "Sorry, the page you are looking for does not exist." )
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

				.Build ();
	}
}