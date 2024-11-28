import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import ApiLinks from '../../assets/api/link.api';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5177/api/';
  CurrentUser = signal<User | null>(null);

  login(model: any) {
    return this.http
      .post<User>(this.baseUrl + 'Student/student-login', model)
      .pipe(
        map((user) => {
          if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.CurrentUser.set(user);
          }
        })
      );
  }
  register(model: any) {
    return this.http
      .post<User>(this.baseUrl + 'Student/student-register', model)
      .pipe(
        map((user) => {
          if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.CurrentUser.set(user);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.CurrentUser.set(null);
  }

  //Method for registering a new user as a tutor
  registerUserAsTutor(email: string, password: string) {
    const requestBody = {
      email: email,
      password: password,
    };

    const response = this.http.post(ApiLinks.tutorRegister, requestBody);
    return response;
  }

  //Method for verification of User email
  verifyEmail(email: string, code: string) {
    const requestBody = {
      email: email,
      verificationCode: code,
    };

    const response = this.http.post(
      ApiLinks.tutorEmailVerification,
      requestBody
    );
    return response;
  }

  //Method for resending verification code
  resendVerificationCode(email: string) {
    const requestBody = {
      email: email,
    };
    const response = this.http.post(
      ApiLinks.tutorEmailVerificationCodeResend,
      requestBody
    );

    return response;
  }
}
