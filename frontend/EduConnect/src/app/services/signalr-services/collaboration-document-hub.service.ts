import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {
  BehaviorSubject,
  debounce,
  debounceTime,
  from,
  Observable,
  Subject,
} from 'rxjs';
import { GetActiveUsersResponse } from './get-active-users-response';
import { GetDocumentContentResponse } from '../../models/shared/signalr/collaboration-document-hub/get-document-content-response';
import { DocumentDelta } from '../../models/shared/signalr/collaboration-document-hub/document-delta';
import { DocumentUpdateRequest } from '../../models/shared/signalr/collaboration-document-hub/document-update-request';
import { GetDocumentResponse } from '../../models/shared/signalr/collaboration-document-hub/get-document-response';
@Injectable({
  providedIn: 'root',
})
export class CollaborationDocumentHubService {
  private connection!: signalR.HubConnection;
  private userJoinedPromiseMap = new Map<string, Promise<void>>();

  private activeUsersSubject = new BehaviorSubject<GetActiveUsersResponse[]>(
    []
  );
  public activeUsers$ = this.activeUsersSubject.asObservable();

  private documentId: string = '';

  public documentContentSubject = new BehaviorSubject<string>('');
  public documentContent$ = this.documentContentSubject.asObservable();

  private updateQueue: DocumentDelta[] = [];
  private currentVersion = 1;
  private isLocalUpdate = false;
  private sendUpdatesSubject = new Subject<void>();

  constructor() {}

  startConnection(documentId: string): Promise<void> {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5177/hubs/collaboration-document', {
        accessTokenFactory: () =>
          localStorage.getItem('Authorization')?.replace('Bearer ', '') || '',
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    this.documentId = documentId;
    // Register event handlers once during connection setup
    this.connection.on('UserJoined', (data) => {
      console.log('User joined:', data);
    });

    this.connection.on(
      'ActiveCollaborators',
      (data: GetActiveUsersResponse[]) => {
        console.log('Active collaborators:', data);
        this.activeUsersSubject.next(data);
      }
    );

    const userJoinedPromise = new Promise<void>((resolve) => {
      this.connection.on('UserJoined', () => {
        resolve();
      });
    });

    this.userJoinedPromiseMap.set(documentId, userJoinedPromise);

    this.connection.on('GetDocumentUpdate', (data) => {
      console.log('Received document update:', data);
      if (!this.isLocalUpdate) {
        this.documentContentSubject.next(data);
      }
    });

    return this.connection.start().then(() => {
      this.connection.invoke('JoinDocumentGroup', documentId);
    });
  }

  async getActiveUsers(documentId: string): Promise<void> {
    await this.userJoinedPromiseMap.get(documentId);
    this.connection.invoke('GetActiveDocumentCollaborators', documentId);
  }

  async leaveDocumentGroup(documentId: string): Promise<void> {
    await this.connection.invoke('LeaveDocumentGroup', documentId);
  }

  stopConnection(): Promise<void> {
    console.log('Stopping SignalR connection');
    return this.connection?.stop();
  }

  getInitialDocumentContent() {
    this.connection.on(
      'GetInitialDocumentContent',
      (data: GetDocumentResponse) => {
        console.log('Initial document content received:', data);
        this.documentContentSubject.next(data.content);
      }
    );
  }

  sendDocumentContentUpdate(content: string) {
    console.log('Sending document content update:', content);
    this.isLocalUpdate = true;
    this.connection
      .invoke('UpdateDocumentContent', this.documentId, content)
      .then(() => {
        this.isLocalUpdate = false;
      });
  }
}
