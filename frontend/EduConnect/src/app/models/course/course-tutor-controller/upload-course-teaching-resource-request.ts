export interface UploadCourseTeachingResourceRequest {
  courseTeachingResourceId: string | null;
  courseId: string | null;
  title: string;
  description: string;
  resourceUrl?: string | null;
  resourceFile?: File | null;
}
