namespace ContactSystem.Core.Domain.Validation;

using Regex;
using FluentValidation;

public static class Rules
{
	public static IRuleBuilderOptions<T , string?> Phone<T> ( this IRuleBuilder<T , string?> ruleBuilder )
		=> ruleBuilder.Matches ( ValidationSet.IsPhoneRegex () );
}