namespace ContactSystem.Api.Common.Extensions;

using Domain.Shared.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

public static class FilterContextExtensions
{
	public static bool HasAuthorization ( this FilterContext filterContext )
		=> filterContext.ActionDescriptor.EndpointMetadata.ContainType<AuthorizeAttribute> ();
}