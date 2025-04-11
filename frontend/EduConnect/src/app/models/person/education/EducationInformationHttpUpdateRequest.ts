export default interface EducationInformationHttpUpdateRequest {
  personEducationInformationId: string;
  institutionName: string | null;
  institutionOfficialWebsite: string | null;
  institutionAddress: string | null;
  educationLevel: string | null;
  fieldOfStudy: string | null;
  minorFieldOfStudy: string | null;
  startDate: string | null;
  endDate: string | null;
  isCompleted: boolean | null;
  finalGrade: string | null;
  description: string | null;
}
