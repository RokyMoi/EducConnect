import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import ITutorRegistrationStatus from '../../_models/Tutor/tutorRegistrationStatus.tutor';
import { TutorRegistrationStatusService } from '../../services/tutor/tutor-status/tutor-status.service';
import { User } from '../../_models/User';

@Component({
  standalone: true,
  selector: 'app-tutor-dashboard',
  imports: [],
 
  templateUrl: './tutor-dashboard.component.html',
  styleUrl: './tutor-dashboard.component.css',
})
export class TutorDashboardComponent {
  accountService = inject(AccountService);
  tutorRegistrationStatusService = inject(TutorRegistrationStatusService);
  tutorRegistrationStatus: ITutorRegistrationStatus | undefined;
  tutorRegistrationStatusList: ITutorRegistrationStatus[] = [];
  userToken: string = '';

  ngOnInit(): void {
    this.accountService.checkAuthentication();
    this.userToken = this.accountService.getAccessToken()!;
    this.tutorRegistrationStatusService.checkTutorRegistrationStatus();
  }
}
