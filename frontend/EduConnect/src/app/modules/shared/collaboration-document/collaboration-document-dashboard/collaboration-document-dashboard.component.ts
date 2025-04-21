import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { CustomHeaderNgContentDialogBoxComponent } from '../../custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { CommonModule } from '@angular/common';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormControl,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { CollaborationDocumentControllerService } from '../../../../services/collaboration/collaboration-document-controller.service';
import {
  debounceTime,
  distinctUntilChanged,
  finalize,
  map,
  Observable,
  of,
  switchMap,
} from 'rxjs';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { GetAllDocumentsByCreatedByPersonIdResponse } from '../../../../models/shared/collaboration-document-controller/get-all-documents-by-created-by-person-id-response';
import { GetAllInvitationsForPersonIdResponse } from '../../../../models/shared/collaboration-document-controller/get-all-invitations-for-person-id-response';
import { GetAllDocumentsByParticipantPersonIdResponse } from '../../../../models/shared/collaboration-document-controller/get-all-documents-by-participant-person-id-response';
import { GetAllInvitationsSentByPersonIdResponse } from '../../../../models/shared/collaboration-document-controller/get-all-invitations-sent-by-person-id-response';

@Component({
  selector: 'app-collaboration-document-dashboard',
  standalone: true,
  imports: [
    CustomHeaderNgContentDialogBoxComponent,
    CommonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './collaboration-document-dashboard.component.html',
  styleUrl: './collaboration-document-dashboard.component.css',
})
export class CollaborationDocumentDashboardComponent implements OnInit {
  showCreateDialog: boolean = false;

  showAcceptInvitationDialog: boolean = false;
  showRejectInvitationDialog: boolean = false;

  acceptInvitationDialogMessage: string = '';
  rejectInvitationDialogMessage: string = '';

  showDeleteInviteDialog: boolean = false;
  deleteInviteDialogMessage: string = '';

  selectedInvitationId: string = '';

  isCheckingTitle: boolean = false;

  documentsByUser: GetAllDocumentsByCreatedByPersonIdResponse[] = [];
  documentsWhereUserIsParticipant: GetAllDocumentsByParticipantPersonIdResponse[] =
    [];
  invitationsForUser: GetAllInvitationsForPersonIdResponse[] = [];
  invitationsSentByUser: GetAllInvitationsSentByPersonIdResponse[] = [];
  documentNameFormControl: FormControl<string | null> = new FormControl(null, {
    validators: [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(255),
    ],
    asyncValidators: [this.titleExistsValidator()],
  });
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private collaborationDocumentControllerService: CollaborationDocumentControllerService,
    private snackboxService: SnackboxService
  ) {}
  ngOnInit(): void {
    this.fetchDocumentsByUser();
    this.fetchDocumentsWhereUserIsParticipant();
    this.fetchInvitationsForUser();
    this.fetchInvitationsSentByUser();
  }

  goBack() {
    this.router.navigate(['/tutor/dashboard']);
  }

  onCreateNewDocument() {
    this.showCreateDialog = true;
  }

  cancelDialog() {
    this.showCreateDialog = false;
  }
  createDocument() {
    this.showCreateDialog = false;
    this.collaborationDocumentControllerService
      .createDocument({
        title: this.documentNameFormControl.value as string,
      })
      .subscribe({
        next: (response) => {
          console.log(response);
          this.snackboxService.showSnackbox(
            'Document created successfully',
            'success'
          );
          this.router.navigate([
            '/tutor/collaboration/document',
            response.data,
          ]);
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error creating document${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  titleExistsValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value || control.value.trim() === '') {
        return of(null);
      }

      this.isCheckingTitle = true;

      return of(control.value).pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap((title) =>
          this.collaborationDocumentControllerService
            .checkIsDocumentTitleTaken(title)
            .pipe(finalize(() => (this.isCheckingTitle = false)))
        ),
        map((response) => {
          return response.data ? { titleExists: true } : null;
        })
      );
    };
  }

  onOpenDocument(documentId: string) {
    this.router.navigate(['/tutor/collaboration/document', documentId]);
  }

  fetchDocumentsByUser() {
    this.collaborationDocumentControllerService
      .getAllDocumentsByPersonId()
      .subscribe({
        next: (response) => {
          console.log(response);
          this.documentsByUser = response.data;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error fetching documents${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  fetchDocumentsWhereUserIsParticipant() {
    this.collaborationDocumentControllerService
      .getAllDocumentsByParticipantPersonId()
      .subscribe({
        next: (response) => {
          console.log(response);
          this.documentsWhereUserIsParticipant = response.data;
        },
        error: (error) => {
          console.error(error);
        },
      });
  }

  fetchInvitationsForUser() {
    this.collaborationDocumentControllerService
      .getAllInvitationsForPersonId()
      .subscribe({
        next: (response) => {
          console.log(response);
          this.invitationsForUser = response.data;
        },
        error: (error) => {
          console.error(error);
        },
      });
  }

  fetchInvitationsSentByUser() {
    this.collaborationDocumentControllerService
      .getAllInvitationsSentByPersonId()
      .subscribe({
        next: (response) => {
          console.log(response);
          this.invitationsSentByUser = response.data;
        },
        error: (error) => {
          console.error(error);
        },
      });
  }

  getStatusText(status: boolean | null): string {
    if (status === null) return 'pending';
    return status ? 'accepted' : 'rejected';
  }

  onAcceptInvitation(invitationId: string, title: string) {
    this.acceptInvitationDialogMessage = `Accept invitation for ${title}?`;
    this.selectedInvitationId = invitationId;
    console.log(this.selectedInvitationId);
    this.showAcceptInvitationDialog = true;
  }
  onRejectInvitation(invitationId: string, title: string) {
    this.rejectInvitationDialogMessage = `Reject invitation for ${title}?`;
    this.selectedInvitationId = invitationId;
    console.log(this.selectedInvitationId);
    this.showRejectInvitationDialog = true;
  }

  onCancelAcceptInvitation() {
    this.selectedInvitationId = '';
    this.acceptInvitationDialogMessage = '';
    this.showAcceptInvitationDialog = false;
  }
  onCancelRejectInvitation() {
    this.selectedInvitationId = '';
    this.rejectInvitationDialogMessage = '';
    this.showRejectInvitationDialog = false;
  }

  onDeleteInvite(invitationId: string, title: string, sentToUserData: string) {
    this.deleteInviteDialogMessage = `Delete invitation for ${title} sent to ${sentToUserData}?`;
    this.selectedInvitationId = invitationId;
    console.log(this.selectedInvitationId);
    this.showDeleteInviteDialog = true;
  }

  onCancelDeleteInvite() {
    this.selectedInvitationId = '';
    this.deleteInviteDialogMessage = '';
    this.showDeleteInviteDialog = false;
  }

  acceptInvitation() {
    if (!this.selectedInvitationId.trim()) return;
    this.showAcceptInvitationDialog = false;
    this.collaborationDocumentControllerService
      .acceptInvitation(this.selectedInvitationId)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.snackboxService.showSnackbox('Invitation accepted', 'success');
          this.fetchInvitationsForUser();
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error accepting invitation${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  rejectInvitation() {
    if (!this.selectedInvitationId.trim()) return;
    this.showRejectInvitationDialog = false;
    this.collaborationDocumentControllerService
      .rejectInvitation(this.selectedInvitationId)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.snackboxService.showSnackbox('Invitation rejected', 'success');
          this.fetchInvitationsForUser();
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error accepting invitation${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  deleteInvitation(invitationId: string) {
    this.collaborationDocumentControllerService
      .deleteInvitation(invitationId)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.snackboxService.showSnackbox('Invitation deleted', 'success');
          this.fetchInvitationsForUser();
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error deleting invitation${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  deleteInvite() {
    if (!this.selectedInvitationId.trim()) return;
    this.showDeleteInviteDialog = false;
    this.collaborationDocumentControllerService
      .deleteInviteSentByPersonByInvitationId(this.selectedInvitationId)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.snackboxService.showSnackbox(
            'Invite to the document deleted',
            'success'
          );
          this.fetchInvitationsSentByUser();
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Error deleting invitation${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }
}
