import { PublishedStatus } from '../../../../enums/published-status.enum';

export interface CreateCourseLessonRequest {
  courseId: string;
  courseLessonId: string | null;
  title: string;
  shortSummary: string;
  description: string;
  topic: string;
  content: string;
  lessonSequenceOrder: number;
}
