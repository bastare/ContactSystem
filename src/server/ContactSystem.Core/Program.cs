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

webApplication.MapGet (
	pattern: "/api/v1/contacts" ,
	handler: ContactEndpoints.GetAllAsync )
.WithOpenApi ();

webApplication.MapPost (
	pattern: "/api/v1/contacts" ,
	handler: ContactEndpoints.CreateAsync )
.WithOpenApi ();

webApplication.MapDelete (
	pattern: "/api/v1/contacts/{contactId:long}" ,
	handler: ContactEndpoints.RemoveAsync )
.WithOpenApi ();

webApplication.MapPatch (
	pattern: "/api/v1/contacts/{contactId:long}" ,
	handler: ContactEndpoints.PatchAsync )
.WithOpenApi ();

await webApplication.RunAsync ();