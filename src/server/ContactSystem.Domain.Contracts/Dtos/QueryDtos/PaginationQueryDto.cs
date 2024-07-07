namespace ContactSystem.Domain.Contracts.Dtos.QueryDtos;

public sealed record PaginationQueryDto ( ulong Offset = 1 , ulong Limit = 100 );