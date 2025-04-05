export interface UploadCourseLessonResourceRequest {
  courseLessonResourceId: string | null;
  courseLessonId: string | null;
  title: string;
  description: string;
  resourceUrl: string | null;
  file: File | null;
}
