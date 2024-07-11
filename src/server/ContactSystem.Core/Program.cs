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

var apiVersionSet =
	webApplication
		.NewApiVersionSet ()
		.HasApiVersion ( apiVersion: new ( 1.0 ) )
		.ReportApiVersions ()
		.Build ();

var routeGroupBuilder =
	webApplication
		.MapGroup ( "api/v{apiVersion:apiVersion}" )
		.WithApiVersionSet ( apiVersionSet )
		.WithOpenApi ()
		.AddEndpointFilter<ValidationFilter> ();

ContactEndpoints.MapEndpoints ( routeGroupBuilder );

await webApplication.RunAsync ();