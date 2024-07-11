namespace ContactSystem.Core.Api.Endpoints.Interfaces;

public interface IHasEndpoints
{
	abstract static void MapEndpoints ( RouteGroupBuilder routeGroupBuilder );
}