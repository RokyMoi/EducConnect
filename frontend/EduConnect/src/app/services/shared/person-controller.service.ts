import { Injectable } from '@angular/core';
import ApiLinks from '../../../assets/api/link.api';
import { HttpClient } from '@angular/common/http';
import { UserLoginRequest } from '../../models/person/person-controller/user-login-request';
import { DefaultServerResponse } from '../../models/shared/default-server-response';

@Injectable({
  providedIn: 'root',
})
export class PersonControllerService {
  private apiUrl = ApiLinks.PersonControllerUrl;

  constructor(private httpClient: HttpClient) {}

  public loginUser(request: UserLoginRequest) {
    return this.httpClient.post<DefaultServerResponse>(
      this.apiUrl + '/login',
      request,
      {
        observe: 'response',
      }
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
    return this.httpClient.delete<DefaultServerResponse>(
      this.apiUrl + '/logout',
      {
        headers: headers,
      }
    );
  }
}
