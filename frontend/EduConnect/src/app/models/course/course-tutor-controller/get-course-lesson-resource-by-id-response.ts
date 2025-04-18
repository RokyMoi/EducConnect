export interface GetCourseLessonResourceByIdResponse {
  courseLessonResourceId: string;
  courseLessonId: string;
  title: string;
  description: string;
  fileName: string | null;
  contentType: string | null;
  fileSize: number | null;
  resourceUrl: string | null;
  createdAt: string;
}
