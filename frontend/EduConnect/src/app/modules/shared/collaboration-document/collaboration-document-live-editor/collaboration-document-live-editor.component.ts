import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CollaborationDocumentControllerService } from '../../../../services/collaboration/collaboration-document-controller.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import validator from 'validator';
import { CollaborationDocumentHubService } from '../../../../services/signalr-services/collaboration-document-hub.service';
import { GetActiveUsersResponse } from '../../../../services/signalr-services/get-active-users-response';
import { max, Subscription } from 'rxjs';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { Editor, NgxEditorModule, Toolbar } from 'ngx-editor';

import { NgModule } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { style } from '@angular/animations';
import DeltaCalculator from '../../../../helpers/delta-calculator.helper';
import DiffPatchMatch from 'diff-match-patch';

@Component({
  selector: 'app-collaboration-document-live-editor',
  standalone: true,
  imports: [
    CommonModule,
    NgxDocViewerModule,
    NgxEditorModule,
    ReactiveFormsModule,
  ],
  templateUrl: './collaboration-document-live-editor.component.html',
  styleUrl: './collaboration-document-live-editor.component.css',
})
export class CollaborationDocumentLiveEditorComponent
  implements OnInit, OnDestroy
{
  documentId: string = '';
  activeUsers: GetActiveUsersResponse[] = [];
  private subscription: Subscription = new Subscription();

  public editor: Editor = new Editor({
    history: true,
  });

  toolbar: Toolbar = [
    ['bold', 'italic'],
    ['underline', 'strike'],
    ['code', 'blockquote'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['link', 'image'],
    ['text_color', 'background_color'],
    ['align_left', 'align_center', 'align_right', 'align_justify'],
  ];
  editorContent: string = '';

  content: FormControl<string | null> = new FormControl(null);

  private lastContent: string = '';
  private isRemoteUpdate: boolean = false;
  private dmp = new DiffPatchMatch();

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

    this.subscription.add(
      this.collaborationDocumentHubService.documentContent$.subscribe(
        (content) => {
          this.isRemoteUpdate = true;
          this.content.setValue(content, { emitEvent: false });
          setTimeout(() => (this.isRemoteUpdate = false), 0);
        }
      )
    );

    this.collaborationDocumentHubService
      .startConnection(this.documentId)
      .then(() => {
        this.collaborationDocumentHubService.getActiveUsers(this.documentId);
        this.collaborationDocumentHubService.getInitialDocumentContent();
      })
      .catch((error) => {
        console.error('Error starting SignalR connection:', error);
      });

    this.content.valueChanges.subscribe((value) => {
      console.log('Content changed:', value);
      const newContent = value as string;
      const patch = this.dmp.patch_make(this.lastContent, newContent);
      console.log('Patch made:', patch);

      if (this.isRemoteUpdate) return;
      this.collaborationDocumentHubService.sendDocumentContentUpdate(
        this.documentId,
        value as string
      );

      this.lastContent = newContent;
    });
  }

  async ngOnDestroy(): Promise<void> {
    // Clean up subscriptions when component is destroyed
    this.subscription.unsubscribe();
    await this.collaborationDocumentHubService.leaveDocumentGroup(
      this.documentId
    );
    this.collaborationDocumentHubService.stopConnection();
  }

  async goBack() {
    await this.collaborationDocumentHubService.leaveDocumentGroup(
      this.documentId
    );
    this.router.navigate(['/tutor/dashboard']);
  }
}
