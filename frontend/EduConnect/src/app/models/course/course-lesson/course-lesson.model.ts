export interface CourseLesson {
  courseLessonId: string;
  courseId: string;
  lessonTitle: string;
  lessonDescription: string;
  lessonSequenceOrder: number;
  lessonPrerequisites: string;
  lessonObjective: string;
  lessonCompletionTimeInMinutes: number;
  lessonTag: string;
  createdAt: string;
}
