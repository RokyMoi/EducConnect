export interface TutorTeachingStyleSaveHttpRequestTutor {
  description: string | null;
  teachingStyleTypeId: number;
  primaryCommunicationTypeId: number;
  secondaryCommunicationTypeId: number | null;
  primaryEngagementMethodId: number;
  secondaryEngagementMethodId: number | null;
  expectedResponseTime: string | null;
  specialConsiderations: string | null;
}
