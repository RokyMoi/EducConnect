export interface GetAllCourseLessonResourcesResponse {
  courseLessonResourceId: string;
  courseLessonId: string;
  title: string;
  description: string;
  resourceUrl: string | null;
  fileName: string | null;
  contentType: string | null;
  fileSize: number | null;
  createdAt: string;
}
