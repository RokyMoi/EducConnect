export interface GetAllDocumentsByCreatedByPersonIdResponse {
  documentId: string;
  title: string;
  createdAt: string;
  numberOfParticipants: number;
  totalNumberOfInvitedUsers: number;
  numberOfAcceptedInvitations: number;
  numberOfRejectedInvitations: number;
  numberOfPendingInvitations: number;
}
