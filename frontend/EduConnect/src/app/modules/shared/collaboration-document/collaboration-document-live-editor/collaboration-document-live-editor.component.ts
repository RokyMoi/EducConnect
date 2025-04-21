import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CollaborationDocumentControllerService } from '../../../../services/collaboration/collaboration-document-controller.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import validator from 'validator';

@Component({
  selector: 'app-collaboration-document-live-editor',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './collaboration-document-live-editor.component.html',
  styleUrl: './collaboration-document-live-editor.component.css',
})
export class CollaborationDocumentLiveEditorComponent {
  documentId: string = '';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private collaborationDocumentControllerService: CollaborationDocumentControllerService,
    private snackboxService: SnackboxService
  ) {
    this.documentId = this.route.snapshot.paramMap.get('documentId') as string;

    if (!this.documentId || !validator.isUUID(this.documentId)) {
      this.snackboxService.showSnackbox('Invalid document ID', 'error');
      this.router.navigate(['/forbidden']);
    }
  }

  goBack() {
    this.router.navigate(['/tutor/dashboard']);
  }
}
