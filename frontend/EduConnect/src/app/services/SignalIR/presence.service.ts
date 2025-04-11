import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { User } from '../../models/User';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.hubsUrl;
  private hubConnection?: HubConnection;
  onlineUsers = signal<string[]>([]);

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.Token,
      })
      .withAutomaticReconnect()
      .build();
    this.hubConnection.start().catch((error) => console.log('Error: ' + error));
    this.hubConnection.on('UserIsOnline', (email) => {
      console.log('User have been connected', email);
    });
    this.hubConnection.on('UserIsOffline', (email) => {
      console.log('User have been disconnected', email);
    });
    this.hubConnection.on('GetOnlineUsers', (email) => {
      this.onlineUsers.set(email);
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state == HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }
}
