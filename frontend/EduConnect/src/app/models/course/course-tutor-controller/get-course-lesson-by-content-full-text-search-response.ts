import { PublishedStatus } from '../../../../enums/published-status.enum';

export interface GetCourseLessonByContentFullTextSearchResponse {
  courseLessonId: string;
  courseLessonContentId: string;
  title: string;
  topic: string;
  shortSummary: string;
  publishedStatus: PublishedStatus;
  lessonSequenceOrder: number | null;
  createdAt: string;
  statusChangedAt: string | null;
  content: string;
}
