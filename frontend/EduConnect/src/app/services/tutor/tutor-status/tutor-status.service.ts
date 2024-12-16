import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import ApiLinks from '../../../../assets/api/link.api';
import ITutorRegistrationStatus from '../../../_models/Tutor/tutorRegistrationStatus.tutor';
import { AccountService } from '../../account.service';
import { forkJoin } from 'rxjs';

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

          //If the user status is with Id 3, then route the user to enter their phone number
          if (this.currentStatus?.tutorRegistrationStatusId === 3) {
            this.accountService.router.navigateByUrl('/signup/phone-number');
          }
        }
      },
    });
  }
}
