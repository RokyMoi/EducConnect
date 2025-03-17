export default interface EducationInformationHttpSaveResponse {
  educationInformationId: string;
  institutionName: string | null;
  institutionOfficialWebsite: string | null;
  institutionAddress: string | null;
  educationLevel: string;
  fieldOfStudy: string;
  minorFieldOfStudy: string;
  startDate: string | null;
  endDate: string | null;
  isCompleted: boolean;
  finalGrade: string | null;
  description: string | null;
}
