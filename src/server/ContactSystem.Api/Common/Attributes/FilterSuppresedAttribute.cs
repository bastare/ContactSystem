namespace ContactSystem.Api.Common.Attributes;

using System;

[AttributeUsage ( AttributeTargets.Method , AllowMultiple = false )]
public sealed class FilterSuppressedAttribute : Attribute { }