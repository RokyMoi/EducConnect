import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TextAreaInputComponentComponent } from '../../../common/input/text/text-area-input-component/text-area-input-component.component';
import { TextAreaResizeType } from '../../../../enums/textarea-resize-types.enum';
import { AccountService } from '../../../services/account.service';
import { ReferenceService } from '../../../services/reference/reference.service';
import TutorTeachingStyleType from '../../../_models/reference/tutorTeachingStyleType/tutor-teaching-style-type.tutorTeachingStyleType.reference.model';
import { SelectDropdownComponent } from '../../../common/select/select-dropdown/select-dropdown.component';
import { CommunicationType } from '../../../_models/reference/communication-type/communication-type.reference';
import { EngagementMethod } from '../../../_models/reference/engagement-method/engagement-method.reference';
import { TextInputComponentComponent } from '../../../common/input/text/text-input-component/text-input-component.component';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import { FloatingWarningBoxComponent } from '../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import { NgIf } from '@angular/common';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { timeout } from 'rxjs';
import { TutorTeachingStyleSaveHttpRequestTutor } from '../../../_models/Tutor/tutor-teaching-style/tutor-teaching-style-save-http-request.tutor';
import { TutorRegistrationStatusService } from '../../../services/tutor/tutor-status/tutor-status.service';

@Component({
  standalone: true,
  selector: 'app-tutor-teaching-style',
  imports: [
    ReactiveFormsModule,
    TextAreaInputComponentComponent,
    SelectDropdownComponent,
    TextInputComponentComponent,
    SubmitButtonComponent,
    FloatingWarningBoxComponent,
    NgIf,
    MatProgressSpinner,
  ],
  templateUrl: './tutor-teaching-style.component.html',
  styleUrl: './tutor-teaching-style.component.css',
})
export class TutorTeachingStyleComponent implements OnInit {
  accountService: AccountService = inject(AccountService);
  referenceService: ReferenceService = inject(ReferenceService);
  tutorRegistrationStatusService: TutorRegistrationStatusService = inject(
    TutorRegistrationStatusService
  );

  //Array to store the teaching style types fetched from the server
  tutorTeachingStyleTypes: TutorTeachingStyleType[] = [];
  //Array to store the communication types fetched from the server
  communicationTypes: CommunicationType[] = [];
  //Array to store the engagement methods fetched from the server
  engagementMethods: EngagementMethod[] = [];

  //Variables for description field
  descriptionLabel = 'Description (Optional)';
  descriptionPlaceholder = 'Shortly describe your teaching style...';
  descriptionWarning = '';
  descriptionResizeType = TextAreaResizeType.None;

  //Variables for teaching style type field
  teachingStyleTypeLabel = 'How do you teach?';
  teachingStyleTypePlaceholder = 'Select your teaching style...';
  teachingStyleTypeWarning = '';
  //Array to store the mapped values from the tutorTeachingStyleTypes array
  teachingStyleTypeOptions: any[] = [];

  //Variables for primary communication type field
  primaryCommunicationTypeLabel = 'How do you prefer to communicate?';
  primaryCommunicationTypePlaceholder =
    'Select your preferred communication type...';
  primaryCommunicationTypeWarning = '';

  //Variables for secondary communication type field
  secondaryCommunicationTypeLabel =
    "Is there another way you'd like to communicate?";
  secondaryCommunicationTypePlaceholder =
    "Select another way you'd like to communicate...";
  secondaryCommunicationTypeWarning = '';

  //Array to store the mapped values from the communicationTypes array
  communicationTypeOptions: any[] = [];

  //Variables for primary engagement method field
  primaryEngagementMethodLabel =
    'Select how would you like for your students to engage in classes?';
  primaryEngagementMethodPlaceholder = 'Select engagement method...';
  primaryEngagementMethodWarning = '';

  //Variables for secondary engagement method field
  secondaryEngagementMethodLabel =
    "Is there another way you'd like your students to engage in classes?";
  secondaryEngagementMethodPlaceholder = 'Select engagement method...';
  secondaryEngagementMethodWarning = '';

  //Array to store the mapped values from the engagementMethods array
  engagementMethodOptions: any[] = [];

  //Variables for the expected response field
  expectedResponseTimeLabel =
    'Tell us how long can your students expect to wait for a response from you?';
  expectedResponseTimePlaceholder = 'Enter expected response time...';
  expectedResponseTimeWarning = '';

  //Variables for special considerations field
  specialConsiderationsLabel =
    "Are the any considerations you'd like to make for the students?";
  specialConsiderationsPlaceholder = 'Enter special considerations...';
  specialConsiderationsWarning = '';
  specialConsiderationsResizeType = TextAreaResizeType.None;

  //Variables for the save button
  saveButtonLabel = 'Save teaching style';
  saveButtonColor = 'green';

  //Variables for discard all data button
  discardAllDataButtonLabel = 'Discard all data';
  discardAllDataButtonColor = 'red';

  //Variables for the floating warning box
  floatingWarningBoxMessage = '';
  floatingWarningBoxMessageColor = 'red';
  floatingWarningBoxTitle = 'Warning!';
  floatingWarningBoxShow = false;

  //Variables for the floating warning box confirm button
  floatingWarningBoxConfirmButtonLabel = 'Confirm';
  floatingWarningBoxConfirmButtonColor = 'green';

  //Variables for the floating warning box cancel button
  floatingWarningBoxCancelButtonLabel = 'Cancel';
  floatingWarningBoxCancelButtonColor = 'orange';

  //Common variables for the floating warning box buttons
  floatingWarningBoxButtonMargin: string = '12px 0';

  //Flag to determine which of the type of the operation should be performed
  //True - Create operation
  //False - Discard operation
  isCreateOrDiscardOperation: boolean = true;

  //Variables for the message show beneath the form
  formErrorMessage = '';
  formErrorMessageColor = 'red';

  hasErrors: boolean = false;

  //Form group
  tutorTeachingStyleFormGroup = new FormGroup({
    description: new FormControl(''),
    tutorTeachingStyleType: new FormControl('', [Validators.required]),
    primaryCommunicationType: new FormControl('', [Validators.required]),
    secondaryCommunicationType: new FormControl(''),
    primaryEngagementMethod: new FormControl('', [Validators.required]),
    secondaryEngagementMethod: new FormControl(''),
    expectedResponseTime: new FormControl(''),
    specialConsiderations: new FormControl(''),
  });

  //Text to display once the data has been retrieved from the server
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';
  isDataTransmissionActive: boolean = false;
  isDataTransmissionComplete: boolean = false;
  continueButtonText: string = 'Continue';
  continueButtonColor: string = 'blue';
  saveDataOperationResult: boolean = false;

  ngOnInit(): void {
    this.tutorRegistrationStatusService.checkTutorRegistrationStatus();
    this.referenceService.GetTutorTeachingStyleTypes().subscribe((response) => {
      if (response.success === 'true') {
        this.tutorTeachingStyleTypes = response.data;
        console.log(this.tutorTeachingStyleTypes);
        this.teachingStyleTypeOptions = this.tutorTeachingStyleTypes.map(
          (type) => {
            return {
              name: type.name,
              value: type.tutorTeachingStyleTypeId,
            };
          }
        );
      } else {
        console.log(response.message);
      }
    });

    this.referenceService.GetCommunicationTypes().subscribe((response) => {
      if (response.success === 'true') {
        this.communicationTypes = response.data;
        this.communicationTypeOptions = this.communicationTypes.map((type) => {
          return {
            name: type.name,
            value: type.communicationTypeId,
          };
        });
      }
    });

    this.referenceService.GetEngagementMethods().subscribe((response) => {
      if (response.success === 'true') {
        this.engagementMethods = response.data;
        this.engagementMethodOptions = this.engagementMethods.map((method) => {
          return {
            name: method.name,
            value: method.engagementMethodId,
          };
        });
      }
    });

    //Check for the tutor teaching style type field
    this.tutorTeachingStyleFormGroup.controls.tutorTeachingStyleType.valueChanges.subscribe(
      () => {
        this.checkForTutorTeachingStyleTypeErrors();
      }
    );

    //Check for the primary communication type field
    this.tutorTeachingStyleFormGroup.controls.primaryCommunicationType.valueChanges.subscribe(
      () => {
        this.checkForPrimaryCommunicationTypeErrors();
        this.checkIfThePrimaryAndSecondaryCommunicationTypesAreTheSame();
      }
    );

    //Check for the primary engagement method field
    this.tutorTeachingStyleFormGroup.controls.primaryEngagementMethod.valueChanges.subscribe(
      () => {
        this.checkForPrimaryEngagementMethodErrors();
        this.checkIfThePrimaryAndSecondaryEngagementMethodsAreTheSame();
      }
    );

    //Check if the primary communication type is empty and if the primary and secondary communication type are the same
    this.tutorTeachingStyleFormGroup.controls.secondaryCommunicationType.valueChanges.subscribe(
      () => {
        this.checkIfThePrimaryAndSecondaryCommunicationTypesAreTheSame();
      }
    );

    //Check if the primary engagement method is empty and if the primary and secondary engagement methods are the same
    this.tutorTeachingStyleFormGroup.controls.secondaryEngagementMethod.valueChanges.subscribe(
      () => {
        this.checkIfThePrimaryAndSecondaryEngagementMethodsAreTheSame();
      }
    );

    this.tutorTeachingStyleFormGroup.valueChanges.subscribe(() => {
      const primaryCommunicationType =
        this.tutorTeachingStyleFormGroup.controls.primaryCommunicationType
          .value;
      const secondaryCommunicationType =
        this.tutorTeachingStyleFormGroup.controls.secondaryCommunicationType
          .value;
      const primaryEngagementMethod =
        this.tutorTeachingStyleFormGroup.controls.primaryEngagementMethod.value;
      const secondaryEngagementMethod =
        this.tutorTeachingStyleFormGroup.controls.secondaryEngagementMethod
          .value;

      if (secondaryCommunicationType) {
        this.hasErrors =
          primaryCommunicationType === secondaryCommunicationType;
      }
      if (secondaryEngagementMethod) {
        this.hasErrors = primaryEngagementMethod === secondaryEngagementMethod;
      }

      if (!this.hasErrors && this.tutorTeachingStyleFormGroup.valid) {
        this.formErrorMessage = '';
      }
    });
  }

  //Function to handle the save button click event
  onSaveButtonClick() {
    //Check if the form is valid
    this.checkForErrors();
    console.log('Are errors: ' + this.hasErrors);
    if (this.tutorTeachingStyleFormGroup.invalid || this.hasErrors) {
      console.log('Form is invalid');
      this.formErrorMessage = 'Please resolve any errors before saving';
      return;
    }

    this.isCreateOrDiscardOperation = true;
    console.log('Save button clicked');
    this.floatingWarningBoxTitle = 'Save data about your teaching style';
    this.floatingWarningBoxMessage =
      'Are you sure you want to save the data about your teaching style?';
    this.floatingWarningBoxMessageColor = 'green';

    this.floatingWarningBoxConfirmButtonColor = 'green';
    this.floatingWarningBoxConfirmButtonLabel = 'Yes, save the data';

    this.floatingWarningBoxCancelButtonColor = 'orange';
    this.floatingWarningBoxCancelButtonLabel = 'No, keep editing';
    this.toggleFloatingWarningBoxVisibility();
  }

  //Function to handle the discard all data button click event
  onDiscardAllDataButtonClick() {
    this.isCreateOrDiscardOperation = false;
    this.floatingWarningBoxTitle = 'Discard all entered data';
    this.floatingWarningBoxMessage =
      'Are you sure you want to discard the data about your teaching style, you will have to start from scratch?';
    this.floatingWarningBoxMessageColor = 'red';

    this.floatingWarningBoxConfirmButtonColor = 'red';
    this.floatingWarningBoxConfirmButtonLabel = 'Yes, discard the data';

    this.floatingWarningBoxCancelButtonColor = 'green';
    this.floatingWarningBoxCancelButtonLabel = 'No, keep editing';
    this.toggleFloatingWarningBoxVisibility();
  }

  //Function to handle the confirmation button click event
  onConfirmButtonClick() {
    if (this.isCreateOrDiscardOperation) {
      this.floatingWarningBoxMessage = 'Saving teaching style data...';
      this.floatingWarningBoxMessageColor = 'green';
      this.isDataTransmissionActive = true;

      this.onSaveData();
    }
    if (!this.isCreateOrDiscardOperation) {
      this.onDiscardData();
      this.toggleFloatingWarningBoxVisibility();
    }
  }
  //Function to handle saving the data
  onSaveData() {
    console.log('Saving data...');
    const saveRequest: TutorTeachingStyleSaveHttpRequestTutor = {
      description: this.tutorTeachingStyleFormGroup.controls.description.value,
      teachingStyleTypeId: Number(
        this.tutorTeachingStyleFormGroup.controls.tutorTeachingStyleType
          .value as string
      ),
      primaryCommunicationTypeId: Number(
        this.tutorTeachingStyleFormGroup.controls.primaryCommunicationType
          .value as string
      ),
      secondaryCommunicationTypeId: this.tutorTeachingStyleFormGroup.controls
        .secondaryCommunicationType.value
        ? Number(
            this.tutorTeachingStyleFormGroup.controls.secondaryCommunicationType
              .value as string
          )
        : null,
      primaryEngagementMethodId: Number(
        this.tutorTeachingStyleFormGroup.controls.primaryEngagementMethod
          .value as string
      ),
      secondaryEngagementMethodId: this.tutorTeachingStyleFormGroup.controls
        .secondaryEngagementMethod.value
        ? Number(
            this.tutorTeachingStyleFormGroup.controls.secondaryEngagementMethod
              .value as string
          )
        : null,
      expectedResponseTime:
        this.tutorTeachingStyleFormGroup.controls.expectedResponseTime.value,
      specialConsiderations:
        this.tutorTeachingStyleFormGroup.controls.specialConsiderations.value,
    };
    console.log(saveRequest);
    this.accountService
      .addTutorTeachingStyle(saveRequest)
      .subscribe((response) => {
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;

        if (response.success === 'true') {
          this.saveDataOperationResult = true;
          this.floatingWarningBoxMessage = 'Your teaching style has been saved';
          this.floatingWarningBoxMessageColor = 'green';
          this.continueButtonText = 'Complete registration';
          this.continueButtonColor = 'green';
        }
        if (response.success === 'false') {
          this.saveDataOperationResult = false;
          this.floatingWarningBoxMessage =
            'An error occurred, ' + response.message;
          this.floatingWarningBoxMessageColor = 'red';
          this.continueButtonText = 'Go back to editing';
          this.continueButtonColor = 'blue';
        }
      });
  }
  onDiscardData() {
    this.tutorTeachingStyleFormGroup.reset();
  }
  //Function to handle the cancel button click event
  onKeepEditing() {
    console.log('Cancel button clicked');
    this.toggleFloatingWarningBoxVisibility();
  }
  //Function to toggle the visibility of the floating warning box
  toggleFloatingWarningBoxVisibility() {
    this.floatingWarningBoxShow = !this.floatingWarningBoxShow;
  }

  checkForTutorTeachingStyleTypeErrors() {
    const errors =
      this.tutorTeachingStyleFormGroup.controls.tutorTeachingStyleType.errors;
    if (errors) {
      if (errors['required']) {
        this.teachingStyleTypeWarning = 'Please select a teaching style type';
      }
    }
    if (!errors) {
      this.teachingStyleTypeWarning = '';
    }
  }

  checkForPrimaryCommunicationTypeErrors() {
    const errors =
      this.tutorTeachingStyleFormGroup.controls.primaryCommunicationType.errors;
    if (errors) {
      if (errors['required']) {
        this.primaryCommunicationTypeWarning =
          'Please select a communication type';
      }
    }
    if (!errors) {
      this.primaryCommunicationTypeWarning = '';
    }
  }

  checkForPrimaryEngagementMethodErrors() {
    const errors =
      this.tutorTeachingStyleFormGroup.controls.primaryEngagementMethod.errors;
    if (errors) {
      if (errors['required']) {
        this.primaryEngagementMethodWarning =
          'Please select an engagement method';
      }
    }
    if (!errors) {
      this.primaryEngagementMethodWarning = '';
    }
  }

  checkForExpectedResponseTimeErrors() {
    const errors =
      this.tutorTeachingStyleFormGroup.controls.expectedResponseTime.errors;
    if (errors) {
      if (errors['required']) {
        this.expectedResponseTimeWarning =
          'Please enter an expected response time';
      }
    }
    if (!errors) {
      this.expectedResponseTimeWarning = '';
    }
  }

  checkIfThePrimaryAndSecondaryCommunicationTypesAreTheSame() {
    const primaryCommunicationType =
      this.tutorTeachingStyleFormGroup.controls.primaryCommunicationType.value;
    const secondaryCommunicationType =
      this.tutorTeachingStyleFormGroup.controls.secondaryCommunicationType
        .value;

    if (!secondaryCommunicationType) {
      this.secondaryCommunicationTypeWarning = '';
      return;
    }
    if (primaryCommunicationType === secondaryCommunicationType) {
      this.secondaryCommunicationTypeWarning =
        'You cannot select the two same communication types';
      this.hasErrors = true;
    }
    if (primaryCommunicationType !== secondaryCommunicationType) {
      this.secondaryCommunicationTypeWarning = '';
    }
  }

  checkIfThePrimaryAndSecondaryEngagementMethodsAreTheSame() {
    const primaryEngagementMethod =
      this.tutorTeachingStyleFormGroup.controls.primaryEngagementMethod.value;
    const secondaryEngagementMethod =
      this.tutorTeachingStyleFormGroup.controls.secondaryEngagementMethod.value;
    console.log(
      'Type of secondary eng method: ',
      typeof secondaryEngagementMethod
    );
    console.log('Secondary eng method: ', secondaryEngagementMethod);

    if (!secondaryEngagementMethod) {
      console.log('Secondary eng method is null');
      this.secondaryEngagementMethodWarning = '';
      return;
    }
    if (primaryEngagementMethod === secondaryEngagementMethod) {
      this.secondaryEngagementMethodWarning =
        'You cannot select the two same engagement methods';
      this.hasErrors = true;
    }
    if (primaryEngagementMethod !== secondaryEngagementMethod) {
      this.secondaryEngagementMethodWarning = '';
    }
  }

  checkForErrors() {
    this.checkForTutorTeachingStyleTypeErrors();
    this.checkForPrimaryCommunicationTypeErrors();
    this.checkForPrimaryEngagementMethodErrors();
    this.checkForExpectedResponseTimeErrors();
    this.checkIfThePrimaryAndSecondaryCommunicationTypesAreTheSame();
    this.checkIfThePrimaryAndSecondaryEngagementMethodsAreTheSame();
  }

  onSavingOperationComplete() {
    this.isDataTransmissionComplete = false;
    if (this.saveDataOperationResult) {
      this.accountService.router.navigateByUrl('/tutor/dashboard');
      console.log('OK');
    }
    if (!this.saveDataOperationResult) {
      this.onKeepEditing();
    }
  }
}
