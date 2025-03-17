import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { catchError, map, Observable, of } from 'rxjs';
import ApiLinks from '../../../assets/api/link.api';
import { DefaultServerResponse } from '../../models/shared/default-server-response';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGuardService implements CanActivate {
  constructor(private router: Router, private httpClient: HttpClient) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    const token = localStorage.getItem('Authorization');
    const requiredRole = route.data['requiredRole'];

    if (!token) {
      this.handleUnauthenticated();
      return of(false);
    }
    return this.checkUserRole(token, requiredRole).pipe(
      map((response) => {
        console.log(
          'User with role ' + requiredRole + ' allowed to access the route.'
        );
        return true;
      }),
      catchError((error) => {
        if (error.status === 403) {
          this.router.navigate(['/forbidden']);
        } else {
          this.router.navigate(['/login']);
        }
        return of(false);
      })
    );
  }

  handleUnauthenticated() {
    this.router.navigate(['/login']);
  }

  checkUserRole(
    token: string,
    requiredRole: string
  ): Observable<DefaultServerResponse> {
    const headers = {
      Authorization: `Bearer ${token}`,
    };
    const body = {
      requiredRole: requiredRole,
    };

    return this.httpClient.post<DefaultServerResponse>(
      ApiLinks.PersonControllerUrl + '/role',
      body,
      { headers }
    );
  }

  checkIsValidToken() {
    const token = localStorage.getItem('Authorization');
    console.log('Auth token:', token);
    if (!token) {
      return of(false);
    }

    console.log('Token found, proceeding with token validation...');
    return this.httpClient
      .get<DefaultServerResponse>(
        ApiLinks.PersonControllerUrl + '/authenticate',
        {
          headers: {
            Authorization: 'Bearer ' + token,
          },
        }
      )
      .pipe(
        map((response) => {
          this.router.navigate([`/${response.data.role}/dashboard`]);
          return true;
        }),
        catchError((error) => {
          return of(false);
        })
      );
  }

  
}
