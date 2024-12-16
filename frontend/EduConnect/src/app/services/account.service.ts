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
  baseUrl = 'http://localhost:5177/';
  CurrentUser = signal<User | null>(null);
  EmailUser = signal<String | null>(null);
  router = inject(Router);

  login(model: any) {
    return this.http
      .post<User>(this.baseUrl + 'person/login', model, {
        observe: 'response',
      })
      .pipe(
        map((response) => {
<<<<<<< HEAD
          const userData = (response.body as any)?.data; 
          console.log('API Response:', userData);
          if (userData) {
            const loggedInUser: User = {
              Email:userData.email,
=======
          const userData = (response.body as any)?.data;

          if (userData) {
            const loggedInUser: User = {
              Email: userData.Email,
>>>>>>> e62e459ed1d7fa44c20fc57eae494a1e84df3398
              Role: userData.role,
              Token: userData.token,
            };

<<<<<<< HEAD

           
            console.log("Setting user in localStorage:", loggedInUser);

            this.CurrentUser.set(loggedInUser);
  
            // Sačuvaj podatke u localStorage
            localStorage.setItem('user', JSON.stringify(loggedInUser));
           
            const storedUser = JSON.parse(localStorage.getItem('user')!);
            console.log(storedUser.Email);  // Trebalo bi da prikaže email
=======
            this.CurrentUser.set(loggedInUser);

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
>>>>>>> e62e459ed1d7fa44c20fc57eae494a1e84df3398
          }

          console.log(response.headers.get('Authorization'));
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

  //Method for setting access token
  setAccessToken(token: string) {
    localStorage.setItem('Authorization', token);
  }

  //Method for getting access token
  getAccessToken() {
    return localStorage.getItem('Authorization');
  }

  /**
   * Checks if the user is currently authenticated by verifying the presence of a token.
   * @returns True if a token is found; otherwise, false.
   */
  isAuthenticated(): boolean {
    const token = localStorage.getItem('Authorization');
    return !!token;
  }

  /**
   * Redirects the user to the login page if they are not authenticated.
   */
  checkAuthentication(): void {
    if (!this.isAuthenticated()) {
      window.alert('You must be logged in to access this page.');
      this.router.navigate(['/user-signin']);
    }
  }

  /**
   * Logout user if a system critical error occurs
   */
  logoutOnError(): void {
    window.alert(
      'An unknown error has occurred during this operation, you will be logged out'
    );
    localStorage.removeItem('Authorization');
    this.CurrentUser.set(null);
    this.router.navigate(['/user-signin']);
  }
}
