namespace ContactSystem.Core.Infrastructure.GlobalExceptionHandler;

using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public static class GlobalExceptionHandlerConfigurator
{
	public static void ExceptionFiltersConfigurator ( IApplicationBuilder applicationBuilder )
	{
		applicationBuilder.Run (
			handler: httpContext =>
				FormErrorResponseAsync (
					httpContext ,
					exception: httpContext.ResolveException () ) );
	}

	public static async Task FormErrorResponseAsync ( HttpContext? httpContext , Exception? exception )
	{
		await ResolveGlobalExceptionHandler ( httpContext! )
			.FormErrorResponseAsync ( httpContext , exception );

		await httpContext!.Response.CompleteAsync ();

		static ExceptionHandlerManager ResolveGlobalExceptionHandler ( HttpContext httpContext )
			=> httpContext.RequestServices.GetRequiredService<ExceptionHandlerManager> ();
	}
}