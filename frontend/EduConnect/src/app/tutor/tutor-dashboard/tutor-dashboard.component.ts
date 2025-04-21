import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { TutorRegistrationStatusService } from '../../services/tutor/tutor-status/tutor-status.service';
import { SubmitButtonComponent } from '../../common/button/submit-button/submit-button.component';

@Component({
  standalone: true,
  selector: 'app-tutor-dashboard',

  imports: [SubmitButtonComponent],

  templateUrl: './tutor-dashboard.component.html',
  styleUrl: './tutor-dashboard.component.css',
})
export class TutorDashboardComponent {
  accountService = inject(AccountService);
  tutorRegistrationStatusService = inject(TutorRegistrationStatusService);

  createCourseButtonText: string = 'Create new course';
  onCreateCreateCourseButtonClick() {
    this.accountService.router.navigateByUrl('/tutor/course/create');
  }

  ngOnInit(): void {
    this.accountService.checkAuthentication();
    this.tutorRegistrationStatusService.checkTutorRegistrationStatus();
  }
}
