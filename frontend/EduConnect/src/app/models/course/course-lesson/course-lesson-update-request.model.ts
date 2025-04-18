export interface CourseLessonUpdateRequest {
  courseLessonId: string;
  lessonTitle: string;
  updateLessonTitle: boolean;
  lessonDescription: string;
  updateLessonDescription: boolean;
  lessonSequenceOrder: number;
  updateLessonSequenceOrder: boolean;
  lessonPrerequisites: string;
  updateLessonPrerequisites: boolean;
  lessonObjective: string;
  updateLessonObjective: boolean;
  lessonCompletionTimeInMinutes: number;
  updateLessonCompletionTimeInMinutes: boolean;
  lessonTag: string;
  updateLessonTag: boolean;
  lessonContentTitle: string;
  updateLessonContentTitle: boolean;
  lessonContentDescription: string;
  updateLessonContentDescription: boolean;
  lessonContentData: string;
  updateLessonContentData: boolean;
}
