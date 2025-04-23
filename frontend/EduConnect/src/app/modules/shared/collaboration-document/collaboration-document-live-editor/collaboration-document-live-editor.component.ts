import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CollaborationDocumentControllerService } from '../../../../services/collaboration/collaboration-document-controller.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import validator from 'validator';
import { CollaborationDocumentHubService } from '../../../../services/signalr-services/collaboration-document-hub.service';
import { GetActiveUsersResponse } from '../../../../services/signalr-services/get-active-users-response';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-collaboration-document-live-editor',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './collaboration-document-live-editor.component.html',
  styleUrl: './collaboration-document-live-editor.component.css',
})
export class CollaborationDocumentLiveEditorComponent
  implements OnInit, OnDestroy
{
  documentId: string = '';
  activeUsers: GetActiveUsersResponse[] = [];
  private subscription: Subscription = new Subscription();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private collaborationDocumentControllerService: CollaborationDocumentControllerService,
    private snackboxService: SnackboxService,
    private collaborationDocumentHubService: CollaborationDocumentHubService
  ) {
    this.documentId = this.route.snapshot.paramMap.get('documentId') as string;

    if (!this.documentId || !validator.isUUID(this.documentId)) {
      this.snackboxService.showSnackbox('Invalid document ID', 'error');
      this.router.navigate(['/forbidden']);
    }
  }

  ngOnInit(): void {
    // Subscribe to active users updates
    this.subscription.add(
      this.collaborationDocumentHubService.activeUsers$.subscribe((users) => {
        this.activeUsers = users;
        console.log('Component received active users:', this.activeUsers);
      })
    );

    this.collaborationDocumentHubService
      .startConnection(this.documentId)
      .then(() =>
        this.collaborationDocumentHubService.getActiveUsers(this.documentId)
      )
      .catch((error) => {
        console.error('Error starting SignalR connection:', error);
      });
  }

  ngOnDestroy(): void {
    // Clean up subscriptions when component is destroyed
    this.subscription.unsubscribe();
  }

  async goBack() {
    await this.collaborationDocumentHubService.leaveDocumentGroup(
      this.documentId
    );
    this.router.navigate(['/tutor/dashboard']);
  }
}
