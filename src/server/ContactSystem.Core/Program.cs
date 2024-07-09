using ContactSystem.Core;
using ContactSystem.Core.Api.Endpoints.v1;

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

var versionSet =
	webApplication
		.NewApiVersionSet ()
		.HasApiVersion ( apiVersion: new ( 1.0 ) )
		.ReportApiVersions ()
		.Build ();

var apiGroup =
	webApplication
		.MapGroup ( "api/v{apiVersion:apiVersion}" )
		.WithApiVersionSet ( versionSet )
		.WithOpenApi ();

var v1Contacts =
	apiGroup
		.MapGroup ( "contacts" )
		.MapToApiVersion ( 1.0 );

v1Contacts.MapGet (
	pattern: string.Empty ,
	handler: ContactEndpoints.GetAllAsync );

v1Contacts.MapPost (
	pattern: string.Empty ,
	handler: ContactEndpoints.CreateAsync );

v1Contacts.MapDelete (
	pattern: "{contactId:long}" ,
	handler: ContactEndpoints.RemoveAsync );

v1Contacts.MapPatch (
	pattern: "{contactId:long}" ,
	handler: ContactEndpoints.PatchAsync );

await webApplication.RunAsync ();