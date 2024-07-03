export type PaginationListDto<TItem> = {
  rows: TItem[];
  currentOffset: number;
  totalPages: number;
  limit: number;
  totalCount: number;
}
