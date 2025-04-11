import { HttpClient } from '@angular/common/http';
import { inject, Injectable, Signal, signal } from '@angular/core';
import { User } from '../models/User';
import { map, Subject, Observable, catchError, of } from 'rxjs';
import ApiLinks from '../../assets/api/link.api';
import { Router } from '@angular/router';

import ErrorHttpResponseData from '../models/data/http.response.data/error.http.response.data';
import EducationInformation from '../models/person/education/educationInformation.education.person';
import SuccessHttpResponseData from '../models/data/http.response.data/success.http.response.data';
import EducationInformationHttpSaveRequest from '../models/person/education/educationInformationSaveRequst';
import EducationInformationHttpSaveResponse from '../models/person/education/educationInformationHttpSaveResponse';
import EducationInformationHttpUpdateRequest from '../models/person/education/EducationInformationHttpUpdateRequest';
import CareerInformationHttpSaveRequest from '../models/person/career/careerInformationHttpSaveRequest';
import CareerInformationHttpUpdateRequest from '../models/person/career/careerInformationHttpUpdateRequest';
import { TimeAvailability } from '../models/person/time-availabilty/time-availability';
import { TimeAvailabilityHttpSaveRequest } from '../models/person/time-availabilty/time-availability-http-save-request';
import { TutorTeachingStyleSaveHttpRequestTutor } from '../models/Tutor/tutor-teaching-style/tutor-teaching-style-save-http-request.tutor';
import { PresenceService } from './SignalIR/presence.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  http = inject(HttpClient);
  baseUrl = 'http://localhost:5177/';
  CurrentUser = signal<User | null>(null);
presenceService = inject(PresenceService);
  router = inject(Router);

  login(model: any) {
    return this.http
      .post<User>(this.baseUrl + 'person/login', model, {
        observe: 'response',
      })
      .pipe(
        map((response) => {
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

  register(model: any) {
    return this.http
      .post<User>(this.baseUrl + 'api/Student/student-register', model, {
        observe: 'response',
      })
      .pipe(
        map((response) => {
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
  logout() {
    localStorage.clear();
    this.CurrentUser.set(null);
    this.presenceService.stopHubConnection();
    
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

  /**
   * Send a request to the server to create a phone number for the logged-in user.
   * @param countryCodeId - The ID of the country associated with the phone number.
   * @param phoneNumber - The phone number to be created.
   * @returns If successful - SuccessHttpResponseData object containing the response data.
   * @return If unsuccessful - ErrorHttpResponseData object containing the error message.
   */
  createPhoneNumber(countryCode: string, phoneNumber: string) {
    const requestBody = {
      nationalCallingCodeCountryId: countryCode,
      phoneNumber: phoneNumber,
    };
    const authorization = this.getAccessToken();
    const resultSubject = new Subject<
      SuccessHttpResponseData | ErrorHttpResponseData
    >();
    this.http
      .post(ApiLinks.addPhoneNumber, requestBody, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .subscribe({
        next: (response) => {
          console.log('Phone number created successfully:', response);
          const successResponse: SuccessHttpResponseData = {
            success: 'true',
            data: (response as any).data,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          };
          resultSubject.next(successResponse);
          resultSubject.complete();
        },
        error: (error) => {
          console.error('Error creating phone number:', error);
          const errorResponse: ErrorHttpResponseData = {
            success: 'false',
            data: null,
            message: error.error.message,
            statusCode: error.status,
            statusText: error.statusText,
          };

          resultSubject.next(errorResponse);
          resultSubject.complete();
        },
      });
    return resultSubject.toPromise();
  }

  /**
   * Send a request to the server to save the given person details for the logged-in user.
   * @param firstName - The first name of the person - optional
   * @param lastName - The last name of the person - optional
   * @param username - The username of the person - required
   * @param countryOfOriginCountryId - The country of origin of the person - optional
   * @returns If successful - SuccessHttpResponseData object containing the response data.
   * @return If unsuccessful - ErrorHttpResponseData object containing the error message.
   */
  createPersonDetails(
    firstName: string = '',
    lastName: string = '',
    username: string,
    countryOfOriginCountryId: string = ''
  ) {
    const authorization = this.getAccessToken();
    const resultSubject = new Subject<
      SuccessHttpResponseData | ErrorHttpResponseData
    >();
    const requestBody = {
      firstName: firstName,
      lastName: lastName,
      username: username,
      countryOfOriginCountryId: countryOfOriginCountryId,
    };
    this.http
      .post(ApiLinks.addPersonDetails, requestBody, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .subscribe({
        next: (response) => {
          console.log('Person details saved:', response);
          const successResponse: SuccessHttpResponseData = {
            success: 'true',
            data: (response as any).data,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          };
          resultSubject.next(successResponse);
          resultSubject.complete();
        },
        error: (error) => {
          console.error('Error creating phone number:', error);
          const errorResponse: ErrorHttpResponseData = {
            success: 'false',
            data: null,
            message: error.error.message,
            statusCode: error.status,
            statusText: error.statusText,
          };

          resultSubject.next(errorResponse);
          resultSubject.complete();
        },
      });
    return resultSubject.toPromise();
  }
  /**
   *
   * @param educationInformation - EducationInformationHttpSaveRequest object containing the education information to be saved
   * @returns Observable of SuccessHttpResponseData or ErrorHttpResponseData
   */
  createPersonEducationInformation(
    educationInformation: EducationInformationHttpSaveRequest
  ): Observable<SuccessHttpResponseData | ErrorHttpResponseData> {
    const authorization = this.getAccessToken();
    return this.http
      .post<EducationInformationHttpSaveResponse>(
        ApiLinks.addEducationInformation,
        educationInformation,
        {
          headers: {
            Authorization: `Bearer ${authorization}`,
          },
        }
      )
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          return {
            success: (response as any).success,
            data: (response as any).data.personEducationInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error(
            'Error creating education information:',
            failedResponse
          );
          return of(failedResponse);
        })
      );
  }

  getAllEducationInformation() {
    const authorization = this.getAccessToken();
    const resultSubject = new Subject<
      SuccessHttpResponseData | ErrorHttpResponseData
    >();

    this.http
      .get(ApiLinks.getAllEducationInformation, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .subscribe({
        next: (response) => {
          console.log('Education information saved:', response);
          const successResponse: SuccessHttpResponseData = {
            success: 'true',
            data: (response as any).data,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          };
          resultSubject.next(successResponse);
          resultSubject.complete();
        },
        error: (error) => {
          console.error('Error saving education data:', error);
          const errorResponse: ErrorHttpResponseData = {
            success: 'false',
            data: null,
            message: error.error.message,
            statusCode: error.status,
            statusText: error.statusText,
          };
          resultSubject.next(errorResponse);
          resultSubject.complete();
        },
      });
    return resultSubject.toPromise();
  }

  /**
   * Updates the education information for the authenticated user.
   *
   * @param educationInformation - The updated education information to be saved.
   * @returns An observable that emits a `SuccessHttpResponseData` or `ErrorHttpResponseData` object, representing the success or failure of the update operation.
   */
  updateEducationInformation(
    educationInformation: EducationInformationHttpUpdateRequest
  ) {
    const authorization = this.getAccessToken();
    return this.http
      .put(ApiLinks.updateEducationInformation, educationInformation, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          return {
            success: (response as any).success,
            data: (response as any).data.educationInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error(
            'Error creating education information:',
            failedResponse
          );
          return of(failedResponse);
        })
      );
  }

  deleteEducationInformation(
    educationInformationId: string
  ): Observable<SuccessHttpResponseData | ErrorHttpResponseData> {
    const authorization = this.getAccessToken();
    const requestBody = {
      personEducationInformationId: educationInformationId,
    };
    return this.http
      .delete(ApiLinks.deleteEducationInformation, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
        body: requestBody,
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          return {
            success: (response as any).success,
            data: (response as any).data.educationInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error(
            'Error creating education information:',
            failedResponse
          );
          return of(failedResponse);
        })
      );
  }

  getAllCareerInformation(): Observable<
    SuccessHttpResponseData | ErrorHttpResponseData
  > {
    const authorization = this.getAccessToken();

    return this.http
      .get(ApiLinks.getAllCareerInformation, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          return {
            success: (response as any).success,
            data: (response as any).data.careerInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          };
        }),
        catchError((error) => {
          return of({
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          });
        })
      );
  }

  createCareerInformation(
    careerInformation: CareerInformationHttpSaveRequest
  ): Observable<SuccessHttpResponseData | ErrorHttpResponseData> {
    const authorization = this.getAccessToken();
    return this.http
      .post(ApiLinks.addCareerInformation, careerInformation, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          return {
            success: (response as any).success,
            data: (response as any).data.careerInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }

  updateCareerInformation(
    careerInformation: CareerInformationHttpUpdateRequest
  ) {
    const authorization = this.getAccessToken();
    return this.http
      .put(ApiLinks.updateCareerInformation, careerInformation, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          return {
            success: (response as any).success,
            data: (response as any).data.careerInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }

  deleteCareerInformation(careerInformationId: string) {
    const requestBody = {
      personCareerInformationId: careerInformationId,
    };
    const authorization = this.getAccessToken();
    return this.http
      .delete(ApiLinks.deleteCareerInformation, {
        body: requestBody,
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          return {
            success: (response as any).success,
            data: (response as any).data.careerInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }

  //PersonAvailabilityController
  createTimeAvailability(timeAvailability: TimeAvailabilityHttpSaveRequest) {
    const authorization = this.getAccessToken();
    return this.http
      .post(ApiLinks.addTimeAvailability, timeAvailability, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          console.log('response', response);
          return {
            success: (response as any).success,
            data: (response as any).data.timeAvailability,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          console.log('error', error);
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }

  getAllTimeAvailability() {
    const authorization = this.getAccessToken();
    return this.http
      .get(ApiLinks.getAllTimeAvailability, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          console.log('response', response);
          return {
            success: (response as any).success,
            data: (response as any).data.timeAvailability,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          console.log('error', error);
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }

  updateTimeAvailability(timeAvailability: TimeAvailability) {
    const authorization = this.getAccessToken();
    console.log('Token: ', authorization);
    return this.http
      .put(ApiLinks.updateTimeAvailability, timeAvailability, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          console.log('response', response);
          return {
            success: (response as any).success,
            data: (response as any).data.timeAvailability,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          console.log('error', error);
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }

  deleteTimeAvailability(timeAvailabilityId: string) {
    const authorization = this.getAccessToken();

    return this.http
      .delete(ApiLinks.deleteTimeAvailability, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
        body: {
          personAvailabilityId: timeAvailabilityId,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          console.log('response', response);
          return {
            success: (response as any).success,
            data: (response as any).data.deletedTimeAvailability,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          console.log('error', error);
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }

  addTutorTeachingStyle(
    tutorTeachingStyle: TutorTeachingStyleSaveHttpRequestTutor
  ) {
    const authorization = this.getAccessToken();
    return this.http
      .post(ApiLinks.addTutorTeachingInformation, tutorTeachingStyle, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          // Transform to SuccessHttpResponseData
          console.log('response', response);
          return {
            success: (response as any).success,
            data: (response as any).data.tutorTeachingInformation,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          } as SuccessHttpResponseData;
        }),
        catchError((error) => {
          // Transform to ErrorHttpResponseData
          console.log('error', error);
          const failedResponse: ErrorHttpResponseData = {
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          };
          console.error('Error creating career information:', failedResponse);
          return of(failedResponse);
        })
      );
  }
}
