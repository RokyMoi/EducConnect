import { DocumentDelta } from './document-delta';

export interface DocumentUpdateRequest {
  documentId: string;
  clientVersion: number;
  deltas: DocumentDelta[];
}
