import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-audio-preview',
  standalone: true,
  imports: [],
  templateUrl: './audio-preview.component.html',
  styleUrl: './audio-preview.component.css',
})
export class AudioPreviewComponent {
  @Input() set file(file: File) {
    this.objectUrl = URL.createObjectURL(file);
  }
  objectUrl!: string;
}
