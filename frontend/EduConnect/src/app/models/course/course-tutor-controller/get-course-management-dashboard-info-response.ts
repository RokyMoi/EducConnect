import { GetCourseTeachingResourceResponse } from './get-course-teaching-resource-response';

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
  totalNumberOfTeachingResources: number;
  numberOfFiles: number;
  numberOfURLs: number;
  totalSizeOfFilesInBytes: number;
  twoLatestAddedTeachingResources: GetCourseTeachingResourceResponse[];
}
