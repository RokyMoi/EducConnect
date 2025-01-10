import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { AvailabilityDayTimeSelectComponent } from '../availability-day-time-select/availability-day-time-select.component';
import { NgIf } from '@angular/common';
import { AccountService } from '../../../../services/account.service';
import { TutorRegistrationStatusService } from '../../../../services/tutor/tutor-status/tutor-status.service';
import { AvailabilityRecordsTableComponent } from '../availability-records-table/availability-records-table.component';
import { TimeAvailability } from '../../../../_models/person/time-availabilty/time-availability';
import { EducationService } from '../../../../services/education/education-service/education-service.service';
import { Subscription } from 'rxjs';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
@Component({
  selector: 'app-availability-signup',
  standalone: true,
  imports: [
    AvailabilityDayTimeSelectComponent,
    NgIf,
    AvailabilityRecordsTableComponent,
    SubmitButtonComponent,
  ],
  templateUrl: './availability-signup.component.html',
  styleUrl: './availability-signup.component.css',
})
export class AvailabilitySignupComponent implements OnInit, OnDestroy {
  accountService = inject(AccountService);
  tutorRegistrationService = inject(TutorRegistrationStatusService);
  educationService = inject(EducationService);

  availabilityRecords: TimeAvailability[] = [];

  isStepCompleted: boolean = false;
  goToNextStepButtonText = 'Go to tutoring preferences';
  gotToNextStepButtonMargin: string = '35% 0';

  private refreshSubscription: Subscription = new Subscription();
  ngOnInit(): void {
    this.tutorRegistrationService.checkTutorRegistrationStatus();
    this.loadTimeAvailability();

    this.refreshSubscription = this.educationService.refresh$.subscribe(() => {
      this.loadTimeAvailability();
    });
  }
  ngOnDestroy(): void {
    this.refreshSubscription.unsubscribe();
  }

  loadTimeAvailability() {
    this.accountService.getAllTimeAvailability().subscribe((response) => {
      if (response.success === 'true') {
        this.availabilityRecords = response.data;
      }
      this.isStepCompleted = this.availabilityRecords.length > 0;
    });
  }

  toggleRefreshTrigger() {
    this.educationService.triggerRefresh();
  }

  onRecordsSorted(sortedRecords: TimeAvailability[]) {
    console.log('Received sorted records:', sortedRecords);
  }

  routeToTheNextStep() {
    this.accountService.router.navigateByUrl('signup/tutor/teaching-style');
  }
}
