import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ImageCompressionService {
  constructor() {}

  async compressImage(
    file: File,
    options: {
      maxWidth?: number;
      maxHeight?: number;
      quality?: number;
      mimeType?: string;
    } = {}
  ): Promise<File> {
    const {
      maxWidth = 800,
      maxHeight = 800,
      quality = 0.7,
      mimeType = 'image/jpeg',
    } = options;

    return new Promise((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = (event: ProgressEvent<FileReader>) => {
        const img = new Image();
        img.onload = () => {
          const canvas = document.createElement('canvas');
          let width = img.width;
          let height = img.height;
          if (width > height) {
            if (width > maxWidth) {
              height = Math.round((height *= maxWidth / width));
              width = maxWidth;
            }
          } else {
            if (height > maxHeight) {
              width = Math.round((width *= maxHeight / height));
              height = maxHeight;
            }
          }

          canvas.width = width;
          canvas.height = height;

          const ctx = canvas.getContext('2d');

          if (!ctx) {
            reject(new Error('Could not get canvas context'));
            return;
          }

          ctx.drawImage(img, 0, 0, width, height);

          canvas.toBlob(
            (blob) => {
              if (!blob) {
                reject(new Error('Image compression failed'));
                return;
              }

              const newFileName = file.name.replace(
                /(\.\w+)$/,
                `.${mimeType.split('/')[1]}`
              );

              const compressedFile = new File([blob], newFileName, {
                type: mimeType,
                lastModified: Date.now(),
              });

              resolve(compressedFile);
            },
            mimeType,
            quality
          );
        };
        img.onerror = (error) => reject(error);
        img.src = event.target?.result as string;
      };

      reader.onerror = (error) => reject(error);
      reader.readAsDataURL(file);
    });
  }
}
