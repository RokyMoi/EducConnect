export interface DefaultServerPaginatedResponse<T = any> {
  message: string;
  data: T;
  timestamp: string;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
}
