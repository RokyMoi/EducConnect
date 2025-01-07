export interface CreateCourseHttpRequestBody {
  courseName: string;
  courseSubject: string;
  courseDescription: string;
  learningSubcategoryId: string;
  learningDifficultyLevelId: number;
  IsDraft: boolean;
  courseTypeId: number;
  coursePrice: number;
}
