using ContactSystem.Core;
using ContactSystem.Core.Api.Endpoints.v1;
using ContactSystem.Core.Api.Filters;

var builder = WebApplication.CreateBuilder (
	options: new ()
	{
		Args = args ,
		WebRootPath = "webroot"
	} );

var startup = new Startup ( builder.Configuration , builder.Environment );

builder.Configuration.AddEnvironmentVariables ();

startup.ConfigureServices ( builder.Services );

var webApplication = builder.Build ();

startup.Configure ( webApplication );

MapEndpoints ( webApplication );

await webApplication.RunAsync ();

static void MapEndpoints ( WebApplication webApplication )
{
	var apiVersionSet_ =
		webApplication
			.NewApiVersionSet ()
			.HasApiVersion ( apiVersion: new ( 1.0 ) )
			.ReportApiVersions ()
			.Build ();

	var routeGroupBuilder_ =
		webApplication
			.MapGroup ( "api/v{apiVersion:apiVersion}" )
			.WithApiVersionSet ( apiVersionSet_ )
			.WithOpenApi ()
			.AddEndpointFilter<ValidationFilter> ();

	ContactEndpoints.MapEndpoints ( routeGroupBuilder_ );
}