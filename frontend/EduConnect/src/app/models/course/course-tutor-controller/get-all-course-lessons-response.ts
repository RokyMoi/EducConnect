import { PublishedStatus } from '../../../../enums/published-status.enum';

export interface GetAllCourseLessonsResponse {
  courseId: string;
  courseLessonId: string;
  title: string;
  shortSummary: string;
  description: string;
  topic: string;
  lessonSequenceOrder: number | null;
  publishedStatus: PublishedStatus;
  statusChangedAt: string | null;
  createdAt: string;
  updatedAt: string | null;
  courseLessonContent: string;
}
