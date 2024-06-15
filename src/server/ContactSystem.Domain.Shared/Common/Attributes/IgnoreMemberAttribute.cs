namespace ContactSystem.Domain.Shared.Common.Attributes;

[AttributeUsage ( AttributeTargets.Property | AttributeTargets.Field )]
public sealed class IgnoreMemberAttribute : Attribute { }