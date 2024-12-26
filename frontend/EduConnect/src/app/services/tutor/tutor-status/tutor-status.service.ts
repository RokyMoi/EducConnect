import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import ApiLinks from '../../../../assets/api/link.api';
import ITutorRegistrationStatus from '../../../_models/Tutor/tutorRegistrationStatus.tutor';
import { AccountService } from '../../account.service';
import { forkJoin, catchError, of, map, Observable } from 'rxjs';
import SuccessHttpResponseData from '../../../_models/data/http.response.data/success.http.response.data';
import ErrorHttpResponseData from '../../../_models/data/http.response.data/error.http.response.data';

@Injectable({
  providedIn: 'root',
})
export class TutorRegistrationStatusService {
  statusList: ITutorRegistrationStatus[] = [];
  currentStatus: ITutorRegistrationStatus | undefined;
  accountService = inject(AccountService);

  constructor(private http: HttpClient) {}

  public getAllTutorRegistrationStatus() {
    return this.http.get<any>(ApiLinks.getAllTutorRegistrationStatus);
  }

  public getCurrentTutorRegistrationStatus() {
    let token = this.accountService.getAccessToken();
    return this.http.get<any>(ApiLinks.getCurrentTutorRegistrationStatus, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  public checkTutorRegistrationStatus() {
    forkJoin({
      allStatuses: this.getAllTutorRegistrationStatus(),
      currentStatus: this.getCurrentTutorRegistrationStatus(),
    }).subscribe({
      next: (response) => {
        //Check if the status list is empty, if it is, then return user to the login screen
        this.currentStatus = (
          response.currentStatus as any
        ).data.tutorRegistrationStatus;
        this.statusList = (
          response.allStatuses as any
        ).data.tutorRegistrationStatus;
        if (this.statusList.length === 0) {
          this.accountService.logoutOnError();
        }
        if (this.currentStatus === undefined) {
          this.accountService.logoutOnError();
        }

        //Check if the user's current status, corresponds to the last item of the status list
        if (
          this.statusList[this.statusList.length - 1]
            .tutorRegistrationStatusId !==
          this.currentStatus?.tutorRegistrationStatusId
        ) {
          //The tutor user has not completed the registration process, so the user will be redirected to the adequate registration step page
          console.log('User has not completed the registration process');

          //If the user status is with Id 2 (Email is verified), then route the user to the next step, which is to enter their phone number (Id 3, Phone Number)
          if (this.currentStatus?.tutorRegistrationStatusId === 2) {
            this.accountService.router.navigateByUrl('/signup/phone-number');
          }

          //If the user status is with Id 3 (User have entered their phone number), then route the user to the next step, which is to enter their personal information (Id 4, Personal information)
          if (this.currentStatus?.tutorRegistrationStatusId === 3) {
            this.accountService.router.navigateByUrl(
              '/signup/personal-information'
            );
          }

          //If the user status is with Id 4 (User have entered their personal information), then route the user to the next step which is to enter their education information (Id 5, Education)
          if (this.currentStatus?.tutorRegistrationStatusId === 4) {
            this.accountService.router.navigateByUrl('/signup/education');
          }

          //If the user status is with Id 5 (User have entered their education information), then route the user to the next step which is to enter their career information (Id 6, Career)
          if (this.currentStatus?.tutorRegistrationStatusId === 5) {
            this.accountService.router.navigateByUrl('/signup/career');
          }

          //If the user status is with Id 6 (User have entered their career information), then route the user to the next step which is to enter their availability during the week (Id 7, Availability)
          if (this.currentStatus?.tutorRegistrationStatusId === 6) {
            this.accountService.router.navigateByUrl('/signup/availability');
          }
        }
      },
    });
  }

  public getTutorRegistrationStatus(
    authorization: string
  ): Observable<SuccessHttpResponseData | ErrorHttpResponseData> {
    return this.http
      .get<ITutorRegistrationStatus>(
        ApiLinks.getCurrentTutorRegistrationStatus,
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
            data: (response as any).data.tutorRegistrationStatus,
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
}
