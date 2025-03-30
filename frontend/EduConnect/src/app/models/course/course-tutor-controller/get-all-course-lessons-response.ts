export interface GetAllCourseLessonsResponse {
  courseId: string;
  courseLessonId: string;
  title: string;
  shortSummary: string;
  description: string;
  topic: string;
  lessonSequenceOrder: number | null;
  publishedStatus: number | null;
  statusChangedAt: string | null;
  createdAt: string;
  updatedAt: string | null;
  content: string;
}
