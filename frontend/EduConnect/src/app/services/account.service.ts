import { HttpClient } from '@angular/common/http';
import { inject, Injectable, Signal, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import ApiLinks from '../../assets/api/link.api';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5177/api/';
  CurrentUser = signal<User | null>(null);

  router = inject(Router);
  

 

  login(model: any) {
    return this.http
      .post<User>(this.baseUrl + 'Student/student-login', model, { observe: 'response' })
      .pipe(
        map((response) => {
          const userData = (response.body as any)?.data; 

          if (userData) {
            const loggedInUser: User = {
              Email:userData.Email,
              Role: userData.role,
              Token: userData.token
            };

           
            this.CurrentUser.set(loggedInUser);

     
            localStorage.setItem('user', JSON.stringify(loggedInUser));
          }

          return response; 
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

