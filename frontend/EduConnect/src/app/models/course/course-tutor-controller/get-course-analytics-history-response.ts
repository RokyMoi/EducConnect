import { CourseAnalyticsHistory } from './course-analytics-history';
export interface GetCourseAnalyticsHistoryResponse {
  usersCameFromFeedCount: number;
  usersCameFromSearchCount: number;
  courseAnalyticsHistory: CourseAnalyticsHistory[];
}
