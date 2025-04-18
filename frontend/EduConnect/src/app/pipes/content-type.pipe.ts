import { Pipe, PipeTransform } from '@angular/core';
import { AllowedFileTypes } from '../constants/allowed-file-types.constant';

@Pipe({
  name: 'contentType',
  standalone: true,
})
export class ContentTypePipe implements PipeTransform {
  transform(file: File): string {
    if (!file) return 'unknown';

    const type = file.type.split('/')[0];
    const subtypes = file.type.split('/')[1]?.toLowerCase();

    if (type === 'image') return 'image';
    if (type === 'video') return 'video';
    if (type === 'audio') return 'audio';
    if (file.type === 'application/pdf') return 'pdf';

    // Fallback for common file types that might not have proper MIME types
    const extension = file.name.split('.').pop()?.toLowerCase();
    if (['pdf'].includes(extension || '')) return 'pdf';

    return 'unknown';
  }
}
