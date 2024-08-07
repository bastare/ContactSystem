namespace ContactSystem.Core.Common.Extensions;

using Humanizer;
using System.Security.Cryptography;

public static class StringExtensions
{
	public static string ToPluralForm ( this string @string )
		=> @string?.Pluralize ( inputIsKnownToBeSingular: false ) ??
			throw new ArgumentNullException ( nameof ( @string ) , "String is null" );

	public static string ToSingularForm ( this string @string )
		=> @string?.Singularize ( inputIsKnownToBePlural: false ) ??
			throw new ArgumentNullException ( nameof ( @string ) , "String is null" );

	public static byte[] ToBase64 ( this string @string )
		=> Convert.FromBase64String ( @string );

	public static string ToSHA256 ( this string @string )
	{
		return GetSHA256Hash ( @string )
			.Aggregate (
				new StringBuilder () ,
				( stringBuilder , hashByte ) =>
					stringBuilder.Append ( hashByte.ToString ( "X2" ) ) )

			.ToString ();

		static byte[] GetSHA256Hash ( string inputString )
			=> SHA256.HashData ( source: Encoding.UTF8.GetBytes ( inputString ) );
	}
}