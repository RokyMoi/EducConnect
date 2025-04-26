import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {
  BehaviorSubject,
  debounce,
  debounceTime,
  from,
  Observable,
} from 'rxjs';
import { GetActiveUsersResponse } from './get-active-users-response';
import { GetDocumentContentResponse } from '../../models/shared/signalr/collaboration-document-hub/get-document-content-response';
import { DocumentDelta } from '../../models/shared/signalr/collaboration-document-hub/document-delta';
import { DocumentUpdateRequest } from '../../models/shared/signalr/collaboration-document-hub/document-update-request';
@Injectable({
  providedIn: 'root',
})
export class CollaborationDocumentHubService {
  private connection!: signalR.HubConnection;
  private userJoinedPromiseMap = new Map<string, Promise<void>>();

  // Add a BehaviorSubject to track active users
  private activeUsersSubject = new BehaviorSubject<GetActiveUsersResponse[]>(
    []
  );
  public activeUsers$ = this.activeUsersSubject.asObservable();

  public contentSubject = new BehaviorSubject<string>('');
  public content$ = this.contentSubject.asObservable();

  //Array representing queue of updates from the client to be sent to the server
  private updateQueue: DocumentDelta[] = [];
  private currentVersion: number = 1;
  private isSending: boolean = false;
  private documentId: string = '';

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

  updateDocumentContent(documentId: string, content: string) {
    console.log('Updating document ' + documentId + ' with content:', content);
    this.connection.invoke('UpdateDocumentContent', documentId, content);
  }

  getDocumentContent(documentId: string) {
    console.log('Getting document ' + documentId + ' content');
    this.connection.on(
      'DocumentContentUpdated',
      (content: GetDocumentContentResponse) => {
        console.log('Received document content:', content);
        this.contentSubject.next(content.content);
      }
    );
  }

  stopConnection(): Promise<void> {
    console.log('Stopping SignalR connection');
    return this.connection?.stop();
  }

  
  
}
