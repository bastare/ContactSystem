namespace ContactSystem.Core.Helpers.AssertGuard;

using Common.Exceptions;
using System.Runtime.CompilerServices;

public static class Guard
{
	public static string NotNullOrEmpty<TException> ( string? @string ,
													  [CallerArgumentExpression ( nameof ( @string ) )] string? variableName = default ,
													  string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null or empty";

		if ( string.IsNullOrEmpty ( @string ) )
			RaiseException<TException> ( variableName! , message );

		return @string!;
	}

	public static string NotNullOrEmpty ( string? @string ,
										  [CallerArgumentExpression ( nameof ( @string ) )] string? variableName = default ,
										  string? message = default )
	{
		message ??= $"`{variableName}` is null or empty";

		if ( string.IsNullOrEmpty ( @string ) )
			throw new ArgumentNullException ( variableName , message );

		return @string!;
	}

	public static TValue NotNull<TValue, TException> ( TValue? value ,
													   [CallerArgumentExpression ( nameof ( value ) )] string? variableName = default ,
													   string? message = default )
		where TValue : class
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null";

		if ( value is null )
			RaiseException<TException> ( message , variableName! );

		return value!;
	}

	public static TValue NotNull<TValue> ( TValue? value ,
										   [CallerArgumentExpression ( nameof ( value ) )] string? variableName = default ,
										   string? message = default )
		where TValue : class
	{
		message ??= $"`{variableName}` is null";

		if ( value is null )
			throw new ArgumentNullException ( variableName , message );

		return value!;
	}

	public static IEnumerable NotNullOrEmpty<TException> ( IEnumerable? collection ,
														   [CallerArgumentExpression ( nameof ( collection ) )] string? variableName = default ,
														   string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null or empty";

		if ( collection.IsNullOrEmpty () )
			RaiseException<TException> ( message , variableName! );

		return collection!;
	}

	public static IEnumerable NotNullOrEmpty ( IEnumerable? collection ,
											   [CallerArgumentExpression ( nameof ( collection ) )] string? variableName = default ,
											   string? message = default )
	{
		message ??= $"`{variableName}` is null or empty";

		if ( collection.IsNullOrEmpty () )
			throw new ArgumentNullException ( variableName , message );

		return collection!;
	}

	private static void RaiseException<TException> ( string message , string variableName )
		where TException : ArgumentException
	{
		throw CreateException ( message , variableName );

		static TException CreateException ( string? message , string? variableName )
		{
			try
			{
				return ( TException ) Activator.CreateInstance ( typeof ( TException ) , message , variableName )!;
			}
			catch ( Exception exception )
			{
				throw new AssertGuardException ( exception.Message , exception );
			}
		}
	}
}