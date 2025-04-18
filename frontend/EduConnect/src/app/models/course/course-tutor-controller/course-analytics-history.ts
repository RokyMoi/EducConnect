export interface CourseAnalyticsHistory {
    courseViewershipDataSnapshotId: string;
    courseId: string;
    totalViews: number;
    numberOfUniqueVisitors: number;
    currentlyViewing: number;
    averageViewDurationInMinutes: number;
    createdAt: string;
}
