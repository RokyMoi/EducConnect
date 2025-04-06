import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-video-preview',
  standalone: true,
  imports: [],
  templateUrl: './video-preview.component.html',
  styleUrl: './video-preview.component.css',
})
export class VideoPreviewComponent {
  @Input() set file(file: File) {
    this.objectUrl = URL.createObjectURL(file);
    console.log(this.objectUrl);
    console.log(file.type);
  }
  objectUrl!: string;
}
