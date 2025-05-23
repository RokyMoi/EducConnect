import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  inject,
  OnDestroy,
  OnInit,
} from '@angular/core';

import { SubmitButtonComponent } from '../../../../../common/button/submit-button/submit-button.component';
import EducationInformation from '../../../../../models/person/education/educationInformation.education.person';
import { EducationLogComponent } from '../education-log/education-log/education-log.component';
import { AccountService } from '../../../../../services/account.service';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { EducationService } from '../../../../../services/education/education-service/education-service.service';
import { TutorRegistrationStatusService } from '../../../../../services/tutor/tutor-status/tutor-status.service';

@Component({
  selector: 'app-education',
  standalone: true,
  imports: [SubmitButtonComponent, EducationLogComponent, CommonModule],
  templateUrl: './education.component.html',
  styleUrl: './education.component.css',
})
export class EducationComponent implements OnInit, OnDestroy {
  accountService = inject(AccountService);
  educationService = inject(EducationService);
  tutorService = inject(TutorRegistrationStatusService);
  router = inject(Router);

  isMaxNumberOfRecordsReached: boolean = false;

  private refreshSubscription: Subscription = new Subscription();
  ngOnInit(): void {
    this.tutorService.checkTutorRegistrationStatus();
    console.log('Education component initialized');
    this.loadEducationInformation();

    this.refreshSubscription = this.educationService.refresh$.subscribe(() => {
      this.loadEducationInformation();
    });
  }

  ngOnDestroy(): void {
    this.refreshSubscription.unsubscribe();
  }
  addEducationDetailsLogButtonText = 'Add Education Details';

  educationLogGroups: EducationInformation[] = [];
  addEducationLogButton: boolean = true;
  gridItems: any[] = [];

  selectedGroup: any = null;
  isEditModalOpen = false;
  selectedEducationLogIndex: number | null = null;

  //Variables for continue to the next step button
  isStepCompleted: boolean = this.educationLogGroups.length > 0;
  continueToNextStepButtonText = 'Continue to the Career Information step';
  continueButtonColor: string = 'blue';

  addNewEducationLog() {
    const newEducationInformation: EducationInformation = {
      educationLevel: '',
      fieldOfStudy: '',
      isCompleted: false,
    };
    this.selectedGroup = null;
    this.educationLogGroups.push(newEducationInformation);
    this.gridItems = [...this.educationLogGroups];
    console.log(
      'New education log added',
      this.educationLogGroups.at(this.educationLogGroups.length - 1)
    );
  }

  onLogClick(log: EducationInformation, logIndex: number) {
    console.log('Log clicked');
    console.log('Selected log:', log);
    console.log('Selected log index:', logIndex);
    console.log(
      'Selected log from the list: ',
      this.educationLogGroups[logIndex]
    );
    console.log(
      'Education information id:',
      this.educationLogGroups[logIndex].personEducationInformationId
    );
    this.selectedEducationLogIndex = logIndex;
    this.selectedGroup = log;
    this.toggleEducationLogVisibility();
  }

  toggleEducationLogVisibility() {
    console.log('Toggle clicked');
    this.isEditModalOpen = !this.isEditModalOpen;
    this.educationService.triggerRefresh();
  }

  toggleConsoleLogOverride() {
    console.log('Toggle clicked');
  }

  loadEducationInformation() {
    console.log('Loading education information...');
    this.accountService
      .getAllEducationInformation()
      .then((result) => {
        if (
          result?.success === 'true' &&
          result.data?.personEducationInformation
        ) {
          // Data found
          const educationInformationArray =
            result.data.personEducationInformation;
          this.educationLogGroups = educationInformationArray;
          this.gridItems = [...this.educationLogGroups];
          console.log('Education information loaded:', this.educationLogGroups);
          this.isMaxNumberOfRecordsReached =
            this.educationLogGroups.length >= 5;
          this.isStepCompleted = this.educationLogGroups.length > 0;
        } else if (result?.success === 'false' && !result.data) {
          // No data found
          console.log('No education data found:', result.message);
          this.educationLogGroups = []; // Clear existing data if any
          this.gridItems = [];
        } else {
          // Handle unexpected or error responses
          console.error('Unexpected response:', result);
        }
      })
      .catch((error) => {
        // Handle errors like network issues
        console.error('Error while loading education information:', error);
      });
  }

  routeToCareerInformation() {
    if (this.isStepCompleted) {
      this.router.navigateByUrl('/signup/career');
    } else {
      console.log('Please complete the Education step before proceeding.');
    }
  }
}
