namespace ContactSystem.Core.Infrastructure.GlobalExceptionHandler.Builders;

using Common.Classes.HttpMessages.Error;
using Common.Interfaces;
using ExceptionHandlers;

public sealed class ExceptionHandlerManagerBuilder : IBuilder<ExceptionHandlerManager>
{
	private readonly Stack<IExceptionHandler> _exceptionHandlers = [];

	private IExceptionHandler _defaultExceptionHandler;

	private ExceptionHandlerManagerBuilder ()
	{
		_defaultExceptionHandler = DefaultExceptionMessage ();

		static ExceptionHandler DefaultExceptionMessage ()
			=> new (
				id: 0 ,
				isAllowedException: ( _ , _ ) => true )
			{
				OnHoldAsync = ( _ , _ , _ ) => Task.CompletedTask ,
				InjectStatusCode = ( _ , _ ) => HttpStatusCode.InternalServerError ,
				InjectExceptionMessage =
					( _ ) =>
						new PageErrorMessage (
							StatusCode: ( int ) HttpStatusCode.InternalServerError ,
							Message: "Internal server error" ,
							Description: "Sorry, something went wrong on our end. We are currently trying to fix the problem" )
			};
	}

	public ExceptionHandlerManagerBuilder WithErrorHandler ( IExceptionHandler exceptionHandler )
		=> this.Tap ( self => { self._exceptionHandlers.Push ( exceptionHandler ); } );

	public ExceptionHandlerManagerBuilder WithDefaultErrorHandler ( IExceptionHandler exceptionHandler )
		=> this.Tap ( self => { self._defaultExceptionHandler = exceptionHandler; } );

	public static ExceptionHandlerManagerBuilder Create ()
		=> new ();

	public ExceptionHandlerManager Build ()
		=> new (
			exceptionHandlers: [ .. _exceptionHandlers ] ,
			defaultExceptionHandler: _defaultExceptionHandler );
}