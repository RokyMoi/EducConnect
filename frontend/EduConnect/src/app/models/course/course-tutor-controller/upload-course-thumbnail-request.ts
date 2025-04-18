export interface UploadCourseThumbnailRequest {
  courseId: string;
  useAzureStorage: boolean;
  thumbnailData: File;
}
