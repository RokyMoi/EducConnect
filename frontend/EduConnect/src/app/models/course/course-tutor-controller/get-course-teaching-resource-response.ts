export interface GetCourseTeachingResourceResponse {
  courseTeachingResourceId: string;
  title: string;
  description: string;
  resourceUrl: string | null;
  fileName: string | null;
  contentType: string | null;
  fileSize: number | null;
  createdAt: string;
}
