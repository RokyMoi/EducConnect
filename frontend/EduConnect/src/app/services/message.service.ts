import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PaginationResult } from '../models/pagination';
import { Message } from '../models/messenger/message';
import {
  setPaginatedResponse,
  setPaginationHeaders,
} from '../models/paginationHelper';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = 'http://localhost:5177/Messenger/GetMessagesForUser';
  private http = inject(HttpClient);
  accService = inject(AccountService);
  paginatedResultForMessaging = signal<PaginationResult<Message[]> | null>(
    null
  );

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
  SendMessageToUser(email: string, content: string) {
    let headers = new HttpHeaders();
    const token = this.accService.getAccessToken();

    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
    }

    return this.http.post<Message>(
      'http://localhost:5177/Messenger/CreateMessageForUser',
      { recipientEmail: email, content: content },
      { headers: headers }
    );
  }
}
