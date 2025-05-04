import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PaginationResult } from '../models/pagination';
import { Message } from '../models/messenger/message';
import { setPaginatedResponse, setPaginationHeaders } from '../models/paginationHelper';
import { AccountService } from './account.service';
import { environment } from '../../environments/environment.development';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { User } from '../models/User';
import { take } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private baseUrl = 'http://localhost:5177/Messenger/GetMessagesForUser';
  private messageThreadUrl = 'http://localhost:5177/Messenger/GetMessageThread';
  private http = inject(HttpClient);
  private hubUrl = environment.hubsUrl;
  private hubConnection?: HubConnection;
  private accService = inject(AccountService);
  private connectionRetryCount = 0;
  private maxRetryAttempts = 5;

  // Signal-based state
  paginatedResultForMessaging = signal<PaginationResult<Message[]> | null>(null);
  messageThread = signal<Message[]>([]);
  connectionStatus = signal<string>('disconnected');

  // Observable for connection status that components can subscribe to
  private connectionStatusSource = new BehaviorSubject<string>('disconnected');
  connectionStatus$ = this.connectionStatusSource.asObservable();

  createHubConnection(user: User, otherEmail: string) {
    // Don't attempt to connect if already connected to the same user
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      console.log('Already connected to hub');
      this.stopHubConnection();
    }

    console.log(`Creating hub connection for ${otherEmail}`);

    // Create new connection with enhanced logging and retry logic
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}message?user=${otherEmail}`, {
        accessTokenFactory: () => {
          console.log('Providing access token for hub connection');
          return user.Token;
        },
        skipNegotiation: false,
        withCredentials: true
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect([0, 2000, 5000, 10000, 20000]) // Configure specific retry intervals
      .build();

    this.attachConnectionHandlers();

    // Set up message handlers
    this.hubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      console.log(`Received message thread with ${messages.length} messages`);
      this.messageThread.set(messages);
    });

    this.hubConnection.on('NewMessage', (message: Message) => {
      console.log('Received new message:', message);
      this.messageThread.update((messages) => [...messages, message]);
    });

    // Start the connection
    this.startConnection();
  }

  private attachConnectionHandlers() {
    if (!this.hubConnection) return;

    this.hubConnection.onreconnecting(error => {
      console.log('Attempting to reconnect to message hub:', error);
      this.updateConnectionStatus('reconnecting');
    });

    this.hubConnection.onreconnected(connectionId => {
      console.log('Reconnected to message hub with connection ID:', connectionId);
      this.updateConnectionStatus('connected');
      this.connectionRetryCount = 0;
    });

    this.hubConnection.onclose(error => {
      console.log('Connection closed:', error);
      this.updateConnectionStatus('disconnected');

      // Manual reconnection if automatic reconnection fails
      if (this.connectionRetryCount < this.maxRetryAttempts) {
        this.connectionRetryCount++;
        console.log(`Manual reconnection attempt ${this.connectionRetryCount} of ${this.maxRetryAttempts}`);
        setTimeout(() => this.startConnection(), 5000);
      }
    });
  }

  private startConnection() {
    if (!this.hubConnection) return;

    this.updateConnectionStatus('connecting');

    this.hubConnection.start()
      .then(() => {
        console.log('Connected to message hub successfully');
        this.updateConnectionStatus('connected');
        this.connectionRetryCount = 0;
      })
      .catch(error => {
        console.error('Error connecting to message hub:', error);
        this.updateConnectionStatus('error');

        // Implementation for retry logic
        if (this.connectionRetryCount < this.maxRetryAttempts) {
          this.connectionRetryCount++;
          console.log(`Retry attempt ${this.connectionRetryCount} of ${this.maxRetryAttempts}`);
          setTimeout(() => this.startConnection(), 5000);
        }
      });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      console.log('Stopping hub connection');
      this.hubConnection.stop()
        .then(() => {
          console.log('Disconnected from message hub');
          this.updateConnectionStatus('disconnected');
        })
        .catch((error) => {
          console.error('Error disconnecting from message hub:', error);
        });
    }
  }

  private updateConnectionStatus(status: string) {
    this.connectionStatus.set(status);
    this.connectionStatusSource.next(status);
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginationHeaders(pageNumber, pageSize);
    let headers = new HttpHeaders();

    const token = this.accService.getAccessToken();
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
      return;
    }

    params = params.append('Container', container);

    return this.http
      .get<Message[]>(this.baseUrl, { observe: 'response', params, headers })
      .subscribe({
        next: (response) => setPaginatedResponse(response, this.paginatedResultForMessaging),
        error: (error) => {
          console.error('Error fetching messages:', error);
          if (error.status === 401) {
            console.error('Authentication error when fetching messages. Check token validity.');
          }
        }
      });
  }

  getMessageThread(email: string) {
    let headers = new HttpHeaders();
    const token = this.accService.getAccessToken();

    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
      return;
    }

    return this.http.get<Message[]>(
      `${this.messageThreadUrl}/${email}`,
      { headers }
    );
  }

  async sendMessageToUser(email: string, content: string) {
    try {
      if (!content.trim()) {
        console.warn('Cannot send empty message');
        return;
      }

      if (this.hubConnection?.state === HubConnectionState.Connected) {
        console.log(`Sending message to ${email}`);
        await this.hubConnection.invoke('SendMessage', {
          RecipientEmail: email,
          Content: content,
        });
      } else {
        console.error(`Cannot send message: Connection state is ${this.hubConnection?.state}`);
        // Reconnect if not connected
        if (this.hubConnection?.state !== HubConnectionState.Connecting &&
          this.hubConnection?.state !== HubConnectionState.Reconnecting) {
          console.log('Attempting to reconnect before sending message');
          this.startConnection();
          throw new Error('Connection not ready. Please try sending again after connection is established.');
        }
      }
    } catch (error) {
      console.error('Error sending message:', error);
      throw error;
    }
  }
}