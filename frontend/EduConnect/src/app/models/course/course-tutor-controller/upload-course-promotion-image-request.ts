export interface UploadCoursePromotionImageRequest {
  courseId: string;
  coursePromotionImageId: string | null;
  image: File;
}
