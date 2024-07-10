namespace ContactSystem.Core.Domain.Core.Queries.Interfaces;

public interface IOrderQuery
{
	bool? IsDescending { get; }

	string? OrderBy { get; }
}