import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PaginationResult } from '../models/pagination';
import { Message } from '../models/messenger/message';
import {
  setPaginatedResponse,
  setPaginationHeaders,
} from '../models/paginationHelper';
import { AccountService } from './account.service';
import { environment } from '../../environments/environment.development';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = 'http://localhost:5177/Messenger/GetMessagesForUser';
  private http = inject(HttpClient);
  hubUrl = environment.hubsUrl;
  HubConnection?: HubConnection;
  accService = inject(AccountService);
  paginatedResultForMessaging = signal<PaginationResult<Message[]> | null>(
    null
  );
  messageThread = signal<Message[]>([]);

  ceateHubConnection(user: User, otherEmail: string) {
    var token = localStorage.getItem('Authorization');
    this.HubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherEmail, {
        accessTokenFactory: () => token as string,
      })
      .withAutomaticReconnect()
      .build();

    this.HubConnection.start().catch((error) => console.log(error));
    this.HubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      console.log('Received messages:', messages);
      this.messageThread.set(messages);
    });
    this.HubConnection.on('NewMessage', (message: Message) => {
      console.log('Added new message:', message);
      this.messageThread.update((messages) => [...messages, message]);
    });
  }
  StopHubConnection() {
    if (this.HubConnection?.state == HubConnectionState.Connected) {
      this.HubConnection.stop().catch((error) => console.log(error));
    }
  }
  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginationHeaders(pageNumber, pageSize);
    let headers = new HttpHeaders();

    const token = this.accService.getAccessToken();
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
    }

    params = params.append('Container', container);

    return this.http
      .get<Message[]>(this.baseUrl, { observe: 'response', params, headers })
      .subscribe({
        next: (response) =>
          setPaginatedResponse(response, this.paginatedResultForMessaging),
      });
  }

  getMessageThread(email: string) {
    let headers = new HttpHeaders();
    const token = this.accService.getAccessToken();

    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
    }

    return this.http.get<Message[]>(
      `http://localhost:5177/Messenger/GetMessageThread/${email}`,
      { headers }
    );
  }
  async SendMessageToUser(email: string, content: string) {
    const token = localStorage.getItem('Authorization');
    console.log('Sending message to user: ' + email, content);
    console.log('Token', token);
    this.HubConnection?.invoke('SendMessage', {
      RecipientEmail: email,
      Content: content,
    });
  }
}
