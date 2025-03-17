import { Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';

import { NgFor, NgIf } from '@angular/common';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { CareerSignupLogComponent } from '../../career-signup-log/career-signup-log.component';
import { Router } from '@angular/router';

import { Subscription } from 'rxjs';



import { TutorRegistrationStatusService } from '../../../../../../services/tutor/tutor-status/tutor-status.service';
import { AccountService } from '../../../../../../services/account.service';
import CareerInformation from '../../../../../../models/person/career/careerInformation';
import { EducationService } from '../../../../../../services/education/education-service/education-service.service';
import { SubmitButtonComponent } from '../../../../../../common/button/submit-button/submit-button.component';

@Component({
  standalone: true,
  selector: 'app-career-signup',
  imports: [
    NgFor,
    NgIf,
    MatSlideToggleModule,
    MatIconModule,
    CareerSignupLogComponent,
    SubmitButtonComponent,
  ],
  templateUrl: './career-signup.component.html',
  styleUrl: './career-signup.component.css',
})
export class CareerSignupComponent implements OnInit, OnDestroy {
  tutorStatusService = inject(TutorRegistrationStatusService);
  accountService = inject(AccountService);
  router = inject(Router);
  educationService = inject(EducationService);

  selectedCareerInformationLog: CareerInformation | null = null;
  selectedCareerInformationLogIndex: number | null = null;
  showCareerInformationLog: boolean = false;

  // Reference to the child component
  @ViewChild(CareerSignupLogComponent)
  careerSignupLogComponent!: CareerSignupLogComponent;

  //Array to store the career information
  careerInformation: CareerInformation[] = [];

  //Flag to indicate if the step has been completed, ie. the user has entered their career information
  isStepCompleted: boolean = false;

  //Variables for continue to the next step button
  continueToNextStepButtonText = 'Organize weekly schedule';
  continueButtonColor: string = 'blue';

  private refreshSubscription: Subscription = new Subscription();

  ngOnInit(): void {
    this.tutorStatusService.checkTutorRegistrationStatus();
    this.loadCareerInformation();

    this.refreshSubscription = this.educationService.refresh$.subscribe(() => {
      this.loadCareerInformation();
    });
  }

  ngOnDestroy(): void {
    this.refreshSubscription.unsubscribe();
  }

  loadCareerInformation() {
    console.log('Loading career information...');
    this.accountService.getAllCareerInformation().subscribe((response) => {
      console.log('Response:', response);

      if (response.success === 'true' && response.data.length > 0) {
        this.careerInformation = response.data;
        this.isStepCompleted = true;
        console.log('Career information loaded:', this.careerInformation);
      }
      if (response.success === 'false' && response.data.length === 0) {
        this.isStepCompleted = false;
        this.careerInformation = [];
        console.log('No career information found.');
      }
      if (response.success === 'false' && !response.data) {
        console.log('Unexpected response:', response);
      }
    });
  }

  generateCareerInformationDescription(careerInfo: CareerInformation) {
    const {
      companyName,
      jobTitle,
      position,
      cityOfEmployment,
      countryOfEmployment,
      startDate,
      endDate,
      industry,
      sector,
    } = careerInfo;

    const startDateFormatted = new Date(startDate).toLocaleDateString();
    const endDateFormatted = endDate
      ? new Date(endDate).toLocaleDateString()
      : 'Present';

    let description = `${jobTitle} at ${companyName}`;
    if (position) {
      description += ` (${position})`;
    }
    description += ` - ${cityOfEmployment}, ${countryOfEmployment}\n`;

    description += `Industry: ${industry}(${sector})\n`;
    description += `Employed from ${startDateFormatted} to ${endDateFormatted}.\n`;
    return description;
  }

  addNewCareerInformation() {
    console.log('Adding new career information...');
    this.selectedCareerInformationLog = null;
    this.selectedCareerInformationLogIndex = null;
    this.showCareerInformationLog = true;
    this.notifyChildToFetchEmploymentTypes();
  }

  onLogClick(log: CareerInformation, logIndex: number) {
    console.log(log);
    console.log(logIndex);
    this.selectedCareerInformationLog = log;
    this.selectedCareerInformationLogIndex = logIndex;
    this.showCareerInformationLog = true;
    this.notifyChildToFetchEmploymentTypes();
  }

  notifyChildToFetchEmploymentTypes() {
    if (this.careerSignupLogComponent) {
      this.careerSignupLogComponent.fetchAllData();
    }
  }

  openCareerInformationLog() {
    this.showCareerInformationLog = true;
    console.log('Opening career information log...');
  }

  closeCareerInformationLog() {
    this.showCareerInformationLog = false;
    this.educationService.triggerRefresh();
  }

  routeToWeeklySchedule() {
    this.router.navigate(['/signup/weekly-schedule']);
  }
}
