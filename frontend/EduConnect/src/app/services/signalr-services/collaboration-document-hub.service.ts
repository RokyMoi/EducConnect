import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, from, Observable } from 'rxjs';
import { GetActiveUsersResponse } from './get-active-users-response';
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

  stopConnection(): Promise<void> {
    console.log('Stopping SignalR connection');
    return this.connection?.stop();
  }
}
