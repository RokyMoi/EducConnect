export interface CourseLessonSupplementaryMaterial {
  courseLessonSupplementaryMaterialId: string;
  courseLessonId: string;
  fileName: string;
  contentType: string;
  contentSize: number;
  data: File;
  dateTimePointOfFileCreation: string;
  createdAt: string;
}
