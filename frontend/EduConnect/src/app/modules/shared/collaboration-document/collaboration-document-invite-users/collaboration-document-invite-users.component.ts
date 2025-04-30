import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CollaborationDocumentControllerService } from '../../../../services/collaboration/collaboration-document-controller.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { GetCollaborationDocumentInviteInfoResponse } from '../../../../models/shared/collaboration-document-controller/get-collaboration-document-invite-info-response';
import { CommonModule } from '@angular/common';
import { GetUsersBySearchQueryResponse } from '../../../../models/shared/collaboration-document-controller/get-users-by-search-query-response';
import { FormsModule, NgModel } from '@angular/forms';
import {
  debounceTime,
  distinctUntilChanged,
  Subject,
  Subscription,
} from 'rxjs';

@Component({
  selector: 'app-collaboration-document-invite-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './collaboration-document-invite-users.component.html',
  styleUrl: './collaboration-document-invite-users.component.css',
})
export class CollaborationDocumentInviteUsersComponent implements OnDestroy {
  documentId: string = '';

  documentInfo: GetCollaborationDocumentInviteInfoResponse | null = null;
  searchQuery: string = '';
  users: GetUsersBySearchQueryResponse[] = [];

  private searchSubject = new Subject<string>();
  private searchSubscription: Subscription;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private collaborationDocumentControllerService: CollaborationDocumentControllerService,
    private snackboxService: SnackboxService
  ) {
    this.documentId = this.activatedRoute.snapshot.params['documentId'];

    this.searchSubscription = this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((query) => {
        this.performSearch(query);
      });
    this.checkOwnership();
  }

  ngOnDestroy(): void {
    // Unsubscribe from the search subscription to prevent memory leaks
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
  }

  goBack() {
    this.router.navigate(['/tutor/collaboration']);
  }

  checkOwnership() {
    this.collaborationDocumentControllerService
      .checkCollaborationDocumentOwner(this.documentId)
      .subscribe({
        next: (response) => {
          if (!response.data) {
            this.snackboxService.showSnackbox(
              'You must be the owner of the document to invite users',
              'error'
            );
            this.router.navigate(['/tutor/collaboration']);
          }
          this.loadDocumentInfo();
          this.onSearchUsers();
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error occurred while validating ownership${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
          this.router.navigate(['/tutor/collaboration']);
        },
      });
  }

  loadDocumentInfo() {
    this.collaborationDocumentControllerService
      .getCollaborationDocumentInviteInfo(this.documentId)
      .subscribe({
        next: (response) => {
          this.documentInfo = response.data;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error occurred while loading document info${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }
  onSearchUsers() {
    this.searchSubject.next(this.searchQuery);
  }
  private performSearch(query: string) {
    this.collaborationDocumentControllerService
      .GetUsersBySearchQuery({
        searchQuery: this.searchQuery,
        documentId: this.documentId,
      })
      .subscribe({
        next: (response) => {
          this.users = response.data;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error occurred while searching users${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  inviteUser(personId: string) {
    this.collaborationDocumentControllerService
      .inviteUserToDocument({
        documentId: this.documentId,
        invitedPersonId: personId,
      })
      .subscribe({
        next: (response) => {
          this.snackboxService.showSnackbox(
            'User invited successfully',
            'success'
          );
          this.loadDocumentInfo();
          this.performSearch(this.searchQuery);
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error occurred while inviting user${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }
}
