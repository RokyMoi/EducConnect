export interface GetAllInvitationsForPersonIdResponse {
  collaborationDocumentInvitationId: string;
  documentId: string;
  title: string;
  documentCreatedAt: string;
  invitationSentByPersonIdentificationData: string;
  invitationStatus: boolean | null;
  invitationStatusChangedAt: string | null;
  invitationSentAt: string;
}
