import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ContentTypePipe } from '../../../../pipes/content-type.pipe';
import { ImagePreviewComponent } from '../image-preview/image-preview.component';
import { VideoPreviewComponent } from '../video-preview/video-preview.component';
import { AudioPreviewComponent } from '../audio-preview/audio-preview.component';
import { PdfPreviewComponent } from '../pdf-preview/pdf-preview.component';

@Component({
  selector: 'app-preview',
  standalone: true,
  imports: [
    CommonModule,
    ContentTypePipe,
    ImagePreviewComponent,
    VideoPreviewComponent,
    AudioPreviewComponent,
    PdfPreviewComponent,
  ],
  templateUrl: './preview.component.html',
  styleUrl: './preview.component.css',
})
export class PreviewComponent {
  @Input() file!: File;
  private objectUrls: string[] = [];

  ngOnDestroy() {
    this.cleanUpObjectUrls();
  }

  createObjectUrl(file: File): string {
    const url = URL.createObjectURL(file);
    this.objectUrls.push(url);
    return url;
  }

  private cleanUpObjectUrls() {
    this.objectUrls.forEach((url) => URL.revokeObjectURL(url));
    this.objectUrls = [];
  }
}
