export interface GetAllInvitationsSentByPersonIdResponse {
  collaborationDocumentInvitationId: string;
  documentId: string;
  title: string;
  documentCreatedAt: string;
  invitationSentToPersonIdentificationData: string;
  invitationStatus: boolean | null;
  invitationStatusChangedAt: string | null;
  invitationSentAt: string;
}
