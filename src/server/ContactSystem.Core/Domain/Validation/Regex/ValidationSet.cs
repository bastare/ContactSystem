namespace ContactSystem.Core.Domain.Validation.Regex;

using System.Text.RegularExpressions;

public static partial class ValidationSet
{
    [GeneratedRegex ( @"^\+?(\d{1,3})?[-. (]*(\d{1,4})[-. )]*(\d{1,4})[-. ]*(\d{1,9})$" )]
    public static partial Regex IsPhoneRegex ();
}
