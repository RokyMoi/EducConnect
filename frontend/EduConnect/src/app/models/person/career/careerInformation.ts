export default interface CareerInformation {
  personCareerInformationId: string;
  companyName: string;
  companyWebsite: string | null;
  jobTitle: string;
  position: string | null;
  cityOfEmployment: string;
  countryOfEmployment: string;
  employmentTypeId: number;
  startDate: Date;
  endDate: Date | null;
  jobDescription: string | null;
  responsibilities: string | null;
  achievements: string | null;
  industryClassificationId: string;
  industry: string;
  sector: string;
  skillsUsed: string;
  workTypeId: number | null;
  additionalInformation: string | null;
}
