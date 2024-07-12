namespace ContactSystem.Core.Domain.Core.Queries.Interfaces;

public interface IDynamicQuery :
	IExpressionQuery,
	IOrderQuery,
	IProjectionQuery,
	IPaginationQuery
{ }