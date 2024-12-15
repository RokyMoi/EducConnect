import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import ITutorRegistrationStatus from '../../_models/Tutor/tutorRegistrationStatus.tutor';

@Component({
  selector: 'app-tutor-dashboard',
  imports: [],
  templateUrl: './tutor-dashboard.component.html',
  styleUrl: './tutor-dashboard.component.css',
})
export class TutorDashboardComponent {
  ngOnInit(): void {
    this.getTutorRegistrationStatus();
  }
  AccountService = inject(AccountService);
  tutorRegistrationStatus: ITutorRegistrationStatus = new ITutorRegistrationStatus;
  //Check tutor registration status
  getTutorRegistrationStatus() {
    const token = localStorage.getItem('Authorization');
    if (token) {
      this.AccountService.getTutorRegistrationStatus(token).subscribe({
        next: (response) => {
          console.log(response);
          let tutorRegistrationStatusResponse = (response as any).data
            .tutorRegistrationStatus;
          console.log(tutorRegistrationStatusResponse);

        },
        error: (error) => {
          console.log(error);
        },
      });
    }
  }
}
