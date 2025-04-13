export interface GetCourseAnalyticsHistoryResponse {
  courseViewershipDataSnapshotId: string;
  courseId: string;
  totalViews: number;
  numberOfUniqueVisitors: number;
  currentlyViewing: number;
  averageViewDurationInMinutes: number;
  createdAt: string;
}
