export interface GetAllCoursesResponse {
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
  publishedStatus: boolean | null;
  createdAt: string;
}
