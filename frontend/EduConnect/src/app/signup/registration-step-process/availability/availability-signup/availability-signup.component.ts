import { Component, inject, OnInit } from '@angular/core';
import { AvailabilityDayTimeSelectComponent } from '../availability-day-time-select/availability-day-time-select.component';
import { NgIf } from '@angular/common';
import { AccountService } from '../../../../services/account.service';
import { TutorRegistrationStatusService } from '../../../../services/tutor/tutor-status/tutor-status.service';
@Component({
  selector: 'app-availability-signup',
  imports: [AvailabilityDayTimeSelectComponent, NgIf],
  templateUrl: './availability-signup.component.html',
  styleUrl: './availability-signup.component.css',
})
export class AvailabilitySignupComponent implements OnInit {
  accountService = inject(AccountService);
  tutorRegistrationService = inject(TutorRegistrationStatusService);
  
  
  ngOnInit(): void {
    this.tutorRegistrationService.checkTutorRegistrationStatus();
  }
}
