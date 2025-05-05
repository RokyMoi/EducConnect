import { FileSourceType } from '../../../../enums/file-source-type.enum';

export interface GetAllFilesUploadedByPersonResponse {
  id: string;
  source: string;
  title: string;
  description: string;
  fileName: string;
  contentType: string;
  fileSize: number | null;
  downloadUrl: string;
  fileSourceType: FileSourceType;
  createdAt: string;
  updatedAt: string | null;
}
