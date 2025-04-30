export interface GetCollaborationDocumentInviteInfoResponse {
  documentId: string;
  title: string;
  createdAt: string;
  numberOfParticipants: number;
  totalNumberOfInvitations: number;
  numberOfAcceptedInvitations: number;
  numberOfRejectedInvitations: number;
  numberOfPendingInvitations: number;
}
