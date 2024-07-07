using ContactSystem.Core;

var builder_ = WebApplication.CreateBuilder (
	options: new ()
	{
		Args = args ,
		WebRootPath = "webroot"
	} );

var startup_ = new Startup ( builder_.Configuration , builder_.Environment );

builder_.Configuration.AddEnvironmentVariables ();

startup_.ConfigureServices ( builder_.Services );

var webApplication_ = builder_.Build ();

startup_.Configure ( webApplication_ );

await webApplication_.RunAsync ();