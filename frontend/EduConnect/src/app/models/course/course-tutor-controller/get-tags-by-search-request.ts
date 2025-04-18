export interface GetTagsBySearchRequest {
  searchQuery: string;
  pageNumber: number;
  pageSize: number;
  containsTagCourseId: string | null;
}
