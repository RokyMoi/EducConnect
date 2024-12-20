export default interface EducationInformationHttpRequest {
  educationInformationId?: string;
  institutionName?: string;
  institutionOfficialWebsite?: string;
  institutionAddress?: string;
  educationLevel: string;
  fieldOfStudy: string;
  minorFieldOfStudy?: string;
  startDate: string;
  endDate?: string;
  isCompleted: boolean;
  finalGrade?: string;
  description?: string;
}
