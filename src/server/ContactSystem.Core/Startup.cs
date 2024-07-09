namespace ContactSystem.Core;

using System.IO.Compression;
using Api.Common.Extensions;
using HeyRed.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.GlobalExceptionHandler;
using Infrastructure.loC;
using Asp.Versioning;

public sealed class Startup ( IConfiguration configuration , IWebHostEnvironment webHostEnvironment )
{
	private readonly IConfiguration _configuration = configuration;

	private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

	public void ConfigureServices ( IServiceCollection serviceCollection )
	{
		serviceCollection
			.AddApiVersioning ( setupAction =>
			{
				setupAction.DefaultApiVersion = new ( 1 , 0 );
				setupAction.ApiVersionReader = new UrlSegmentApiVersionReader ();
			} )
			.AddApiExplorer ( options =>
			{
				options.GroupNameFormat = "'v'V";
				options.SubstituteApiVersionInUrl = true;
			} );

		serviceCollection
			.AddEndpointsApiExplorer ()

			.Configure<BrotliCompressionProviderOptions> ( options => options.Level = CompressionLevel.Fastest )
			.Configure<ApiBehaviorOptions> ( options => options.SuppressModelStateInvalidFilter = true )

			.AddResponseCompression ( options =>
			{
				options.EnableForHttps = true;

				options.Providers.Add<BrotliCompressionProvider> ();

				options.MimeTypes = [
					.. ResponseCompressionDefaults.MimeTypes ,
					MimeTypesMap.GetMimeType("svg"),
					MimeTypesMap.GetMimeType("gif"),
					MimeTypesMap.GetMimeType("html"),
					MimeTypesMap.GetMimeType("txt"),
					MimeTypesMap.GetMimeType("css"),
					MimeTypesMap.GetMimeType("png"),
					MimeTypesMap.GetMimeType("jpg"),
					MimeTypesMap.GetMimeType("js"),
					MimeTypesMap.GetMimeType("json"),
					MimeTypesMap.GetMimeType("ico"),
					MimeTypesMap.GetMimeType("woff"),
					MimeTypesMap.GetMimeType("woff2")
				];
			} )

			.InjectLayersDependency ( _configuration );

		if ( _webHostEnvironment.IsDevelopment () )
		{
			serviceCollection
				.AddSwaggerGen ()
				.AddCors ()
				.AddSpaYarp ();
		}
	}

	public void Configure ( WebApplication webApplication )
	{
		if ( _webHostEnvironment.IsProduction () )
			webApplication.UseSecureHeaders ();

		if ( _webHostEnvironment.IsDevelopment () )
			webApplication
				.UseCors ( builder =>
					builder
						.AllowAnyOrigin ()
						.AllowAnyHeader ()
						.AllowAnyMethod () )
				.UseSwagger ()
				.UseSwaggerUI ();

		webApplication
			.UseResponseCompression ()
			.UseDefaultFiles ()
			.UseStaticFiles (
				options: new ()
				{
					OnPrepareResponse = ( context ) =>
					{
						var headers = context.Context.Response.GetTypedHeaders ();

						headers.CacheControl = new ()
						{
							Public = true ,
							MaxAge = TimeSpan.FromDays ( 30 )
						};
					}
				}
			)
			.UseRedirectValidation ()
			.UseExceptionHandler ( GlobalExceptionHandlerConfigurator.ExceptionFiltersConfigurator )
			.UseStaticFiles ()
			.UseRouting ();

		if ( _webHostEnvironment.IsDevelopment () )
			webApplication.UseSpaYarp ();

		if ( _webHostEnvironment.IsProduction () )
			webApplication.MapFallbackToFile ( "index.html" );
	}
}