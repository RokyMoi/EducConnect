export default interface CareerInformationHttpSaveRequest {
  companyName: string;
  companyWebsite: string | null;
  jobTitle: string;
  position: string | null;
  cityOfEmployment: string;
  countryOfEmployment: string;
  employmentTypeId: number;
  startDate: string;
  endDate: string | null;
  jobDescription: string | null;
  responsibilities: string | null;
  achievements: string | null;
  industryClassificationId: string;
  skillsUsed: string;
  workTypeId: number | null;
  additionalInformation: string | null;
}
