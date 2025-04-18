export interface CreateCourseRequest {
  title: string;
  description: string;
  courseCategoryId: string;
  learningDifficultyLevelId: number;
  price: number;
  minNumberOfStudents: number | null;
  maxNumberOfStudents: number | null;
}
