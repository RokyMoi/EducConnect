export interface GetCourseManagementDashboardInfoResponse {
  courseId: string;
  title: string;
  difficultyLevel: string;
  category: string;
  createdAt: string;
  updatedAt: string | null;
  isThumbnailAdded: boolean;
  thumbnailAddedOn: string | null;
  isUsingAzureStorage: boolean;
}
