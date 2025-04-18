import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-image-preview',
  standalone: true,
  imports: [],
  templateUrl: './image-preview.component.html',
  styleUrl: './image-preview.component.css',
})
export class ImagePreviewComponent {
  @Input() set file(file: File) {
    this.objectUrl = URL.createObjectURL(file);
  }
  objectUrl!: string;
}
