import { inject, Injectable, signal } from '@angular/core';
import ApiLinks from '../../../assets/api/link.api';
import { HttpClient } from '@angular/common/http';
import { UserLoginRequest } from '../../models/person/person-controller/user-login-request';
import { DefaultServerResponse } from '../../models/shared/default-server-response';
import { User } from '../../models/User';
import { map } from 'rxjs';
import { PresenceService } from '../SignalIR/presence.service';

@Injectable({
  providedIn: 'root',
})
export class PersonControllerService {
  private apiUrl = ApiLinks.PersonControllerUrl;
  CurrentUser = signal<User | null>(null);
  presenceService = inject(PresenceService);
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5177/';
  constructor(private httpClient: HttpClient) {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      this.CurrentUser.set(JSON.parse(storedUser));
      console.log('Current user set in account service:', this.CurrentUser());
    }
  }

  public loginUser(request: UserLoginRequest) {
    return this.httpClient
      .post<DefaultServerResponse>(this.apiUrl + '/login', request, {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          console.log('This is data from the login,', response);
          const userData = (response.body as any)?.data;

          if (userData) {
            const loggedInUser: User = {
              Email: userData.email,
              Role: userData.role,
              Token: userData.token,
            };

            this.CurrentUser.set(loggedInUser);
            this.presenceService.createHubConnection(userData);

            localStorage.setItem('user', JSON.stringify(loggedInUser));

            //Set the authorization header
            const token = response.headers.get('Authorization');
            if (token) {
              //Clear out the authorization header, if it exists in the local storage
              localStorage.removeItem('Authorization');
              localStorage.setItem(
                'Authorization',
                token.replace('Bearer ', '')
              );
            }
          }

          console.log(response.headers.get('Authorization'));
          return response;
        })
      );
  }

  public getDashboardUserInfo() {
    const token = localStorage.getItem('Authorization');

    const headers = {
      Authorization: `Bearer ${token}`,
    };

    return this.httpClient.get<DefaultServerResponse>(
      this.apiUrl + '/dashboard/info/user',
      {
        headers: headers,
      }
    );
  }

  logout() {
    const token = localStorage.getItem('Authorization');
    const headers = {
      Authorization: `Bearer ${token}`,
    };
    return this.httpClient
      .delete<DefaultServerResponse>(this.apiUrl + '/logout', {
        headers: headers,
      })
      .pipe(
        map((res) => {
          localStorage.removeItem('Authorization');
          localStorage.removeItem('user');
          this.CurrentUser.set(null);
          this.presenceService.stopHubConnection(); // if needed
          return res;
        })
      );
  }

  //Method for setting access token
  setAccessToken(token: string) {
    localStorage.setItem('Authorization', token);
  }

  //Method for getting access token
  getAccessToken() {
    return localStorage.getItem('Authorization');
  }

  register(model: any) {
    return this.http
      .post<User>(this.baseUrl + 'api/Student/student-register', model, {
        observe: 'response',
      })
      .pipe(
        map((response: any) => {
          const userData = (response.body as any)?.data;

          if (userData) {
            const loggedInUser: User = {
              Email: userData.email,
              Role: userData.role,
              Token: userData.token,
            };

            this.CurrentUser.set(loggedInUser);
            this.presenceService.createHubConnection(userData);

            localStorage.setItem('user', JSON.stringify(loggedInUser));

            // Postavljanje autorizacije u localStorage
            const token = response.headers.get('Authorization');
            if (token) {
              localStorage.removeItem('Authorization');
              localStorage.setItem(
                'Authorization',
                token.replace('Bearer ', '')
              );
            }
          }

          return response;
        })
      );
  }
}
