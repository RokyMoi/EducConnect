import { Component, Inject, inject } from '@angular/core';
import { CustomHeaderNgContentDialogBoxComponent } from '../../custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { PreviewComponent } from '../preview/preview.component';
import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-file-preview-dialog',
  standalone: true,
  imports: [
    CustomHeaderNgContentDialogBoxComponent,
    PreviewComponent,
    MatDialogModule,
  ],
  templateUrl: './file-preview-dialog.component.html',
  styleUrl: './file-preview-dialog.component.css',
})
export class FilePreviewDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { file: File }) {}
}
