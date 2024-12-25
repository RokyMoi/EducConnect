export default interface EducationInformation {
  personEducationInformationId?: string;
  institutionName?: string;
  institutionOfficialWebsite?: string;
  institutionAddress?: string;
  educationLevel: string;
  fieldOfStudy: string;
  minorFieldOfStudy?: string;
  startDate?: Date;
  endDate?: Date;
  isCompleted: boolean;
  finalGrade?: string;
  description?: string;
}
