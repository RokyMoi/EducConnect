export interface DocumentDelta {
  position: number;
  insert: string | null;
  delete: number | null;
}
