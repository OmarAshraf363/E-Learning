/** Mirrors the API's PagedResult<T>. */
export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

/** Mirrors the API's PaginationParams (defaults: page 1, size 3, max 20). */
export interface PaginationParams {
  page?: number;
  pageSize?: number;
}
