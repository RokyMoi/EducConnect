import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { SafePipe } from '../../../../pipes/safe.pipe';

@Component({
  selector: 'app-pdf-preview',
  standalone: true,
  imports: [CommonModule, SafePipe],
  templateUrl: './pdf-preview.component.html',
  styleUrl: './pdf-preview.component.css',
})
export class PdfPreviewComponent {
  @Input() set file(file: File) {
    this.objectUrl = URL.createObjectURL(file);
  }
  objectUrl!: string;
}
