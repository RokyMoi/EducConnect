import { PublishedStatus } from '../../../../enums/published-status.enum';
export interface GetCoursesByQueryResponse {
  courseId: string;
  title: string;
  description: string;
  courseCategoryId: string;
  courseCategoryName: string;
  learningDifficultyLevelId: number;
  learningDifficultyLevelName: string;
  price: number;
  minNumberOfStudents: number | null;
  maxNumberOfStudents: number | null;
  numberOfLessons: number;
  publishedStatus: PublishedStatus;
  createdAt: string;
  hasThumbnail: boolean;
  thumbnailUrl: string | null;
  tutorUsername: string;
  tutorEmail: string;
}
