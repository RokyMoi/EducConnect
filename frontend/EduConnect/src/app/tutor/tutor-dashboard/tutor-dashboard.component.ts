import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import ITutorRegistrationStatus from '../../_models/Tutor/tutorRegistrationStatus.tutor';
import { TutorRegistrationStatusService } from '../../services/tutor/tutor-status/tutor-status.service';
import { CreateCourseComponent } from '../course/create-course/create-course.component';
import { SubmitButtonComponent } from '../../common/button/submit-button/submit-button.component';

@Component({
  standalone: true,
  selector: 'app-tutor-dashboard',
<<<<<<< HEAD
  imports: [],
 
=======

  imports: [CreateCourseComponent, SubmitButtonComponent],


>>>>>>> SignalBranch
  templateUrl: './tutor-dashboard.component.html',
  styleUrl: './tutor-dashboard.component.css',
})
export class TutorDashboardComponent {
  accountService = inject(AccountService);
  tutorRegistrationStatusService = inject(TutorRegistrationStatusService);
  tutorRegistrationStatus: ITutorRegistrationStatus | undefined;
  tutorRegistrationStatusList: ITutorRegistrationStatus[] = [];

  createCourseButtonText: string = 'Create new course';
  onCreateCreateCourseButtonClick() {
    this.accountService.router.navigateByUrl('/tutor/course/create');
  }

  ngOnInit(): void {
    this.accountService.checkAuthentication();
    this.tutorRegistrationStatusService.checkTutorRegistrationStatus();
  }
}
