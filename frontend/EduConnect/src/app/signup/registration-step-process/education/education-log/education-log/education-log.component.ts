import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { TextInputComponentComponent } from '../../../../../common/input/text/text-input-component/text-input-component.component';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormControlOptions,
} from '@angular/forms';
import {
  FormatWidth,
  NgIf,
  CommonModule,
  NgClass,
  NgStyle,
} from '@angular/common';
import { SubmitButtonComponent } from '../../../../../common/button/submit-button/submit-button.component';
import { FloatingWarningBoxComponent } from '../../../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import { ErrorStateMatcher } from '@angular/material/core';
import { AccountService } from '../../../../../services/account.service';
import EducationInformation from '../../../../../_models/person/education/educationInformation.education.person';
import DateHelper from '../../../../../helpers/date.helper/date.helper';
import EducationInformationHttpRequest from '../../../../../_models/person/education/request.educationInformation.education.person';
import { DatePickerComponent } from '../../../../../common/input/date/date-picker/date-picker/date-picker.component';
import { TwoOptionPickerComponent } from '../../../../../common/input/two-option-picker/two-option-picker/two-option-picker.component';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { EducationService } from '../../../../../services/education/education-service/education-service.service';

@Component({
  selector: 'app-education-log',
  imports: [
    TextInputComponentComponent,
    ReactiveFormsModule,
    NgIf,
    SubmitButtonComponent,
    FloatingWarningBoxComponent,
    DatePickerComponent,
    TwoOptionPickerComponent,
    MatProgressSpinner,
  ],
  templateUrl: './education-log.component.html',
  styleUrl: './education-log.component.css',
})
export class EducationLogComponent implements OnInit {
  @Input() toggleFloatingBox: () => void = () => {
    console.log('Education service was called');
  };
  @Input() educationLog: EducationInformation | null = null;
  @Input() educationService!: EducationService;
  accountService = inject(AccountService);

  formErrorMessage: string = '';

  //Variables for institution name field
  institutionNameLabel: string = 'Institution name';
  institutionNameError: string = '';
  institutionNamePlaceholder: string = 'Enter institution name';

  //Variables for institution official website field
  institutionOfficialWebsiteLabel: string = 'Institution official website';
  institutionOfficialWebsiteError: string = '';
  institutionOfficialWebsitePlaceholder: string =
    'Enter official website of the institution';

  //Variables for institution address field
  institutionAddressLabel: string = 'Institution address';
  institutionAddressError: string = '';
  institutionAddressPlaceholder: string =
    'Enter the address of the institution';

  //Variables for education level field
  educationLevelLabel: string = 'Education level';
  educationLevelError: string = '';
  educationLevelPlaceholder: string = 'Enter at which level did you study';

  //Variables for field of study field
  fieldOfStudyLabel: string = 'Field of study';
  fieldOfStudyError: string = '';
  fieldOfStudyPlaceholder: string = 'Enter for which field did you study';

  //Variables for minor field of study field
  minorFieldOfStudyLabel: string = 'Minor field of study';
  minorFieldOfStudyError: string = '';
  minorFieldOfStudyPlaceholder: string =
    'If there is any minor field of study, enter it here';

  //Variables for start date field
  startDateLabel: string = 'Start date';
  startDateError: string = '';
  startDatePlaceholder: string = 'Enter the start date of your education';

  //Variables for end date field
  endDateLabel: string = 'End date';
  endDateError: string = '';
  endDatePlaceholder: string = 'Enter the end date you finished your education';

  //Variables for IsCompleted field

  isCompletedLabel: string = 'Did you complete your education?';
  isCompletedError: string = '';
  isCompletedFirstOptionText: string = 'Completed';
  isCompletedSecondOptionText: string = 'Not completed';
  isCompletedFirstOptionValue: string = 'true';
  isCompletedSecondOptionValue: string = 'false';
  isResponseMaxLengthReachedError: boolean = false;
  //Variables for final grade field
  finalGradeLabel: string = 'Final grade';
  finalGradeError: string = '';
  finalGradePlaceholder: string =
    'Enter if you have a final grade for your education';

  //Variables for description field
  descriptionLabel: string = 'Description';
  descriptionError: string = '';
  descriptionPlaceholder: string =
    'Briefly describe your education or add any additional information here';

  //Form control
  educationInformationFormControl = new FormGroup({
    institutionName: new FormControl(''),
    institutionOfficialWebsite: new FormControl(''),
    institutionAddress: new FormControl(''),
    educationLevel: new FormControl(
      '',

      [Validators.required, Validators.pattern(/^(?=.*[a-zA-Z])[a-zA-Z0-9.]+$/)]
    ),
    fieldOfStudy: new FormControl('', [
      Validators.required,
      Validators.pattern(/^(?=.*[a-zA-Z])[a-zA-Z0-9.]+$/),
    ]),
    minorFieldOfStudy: new FormControl(''),
    startDate: new FormControl(),
    endDate: new FormControl(),
    isCompletedGroup: new FormGroup({
      firstOption: new FormControl(true),
      secondOption: new FormControl(false),
    }),
    finalGrade: new FormControl(''),
    description: new FormControl(''),
  });

  initialValues: Record<string, any> = {};
  changedControls: string[] = [];

  showWarningBox: boolean = false;
  warningBoxTitle: string = '';
  warningBoxMessage: string = '';
  warningBoxYesButtonText: string = '';
  warningBoxYesButtonColor: string = '';
  warningBoxNoButtonText: string = '';
  warningBoxNoButtonColor: string = '';
  warningBoxButtonMargin: string = '15px 0px';
  warningBoxMessageColor: string = 'red';
  warningBoxMessageFontSize: string = '24px';

  //Text to display once the data has been retrieved from the server
  saveOperationResult: string = '';
  saveOperationResultTextColor: string = 'yellow';
  isDataTransmissionActive: boolean = false;
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';
  isDataTransmissionComplete: boolean = false;

  continueButtonText: string = 'Continue';
  continueButtonColor: string = 'blue';

  editButtonText: string = 'Edit';
  editButtonColor: string = 'orange';
  //This variable is used to determine if the user is currently performing an operation of saving or discarding data
  //TRUE - saving data
  //FALSE - discarding data
  currentDataOperation: boolean = false;

  //Variable used as a flag to determine is the education log new or is it an edit of an existing education log
  //TRUE - new education log
  //FALSE - edit of an existing education log
  isCreateOrEditMode: boolean = false;

  //Variables used for data update
  updateButtonText: string = 'Save changes';
  updateButtonColor: string = 'green';

  //Variables used for discard changes
  discardButtonText: string = 'Discard changes';
  discardButtonColor: string = 'red';

  //Variables used for go back
  goBackButtonText: string = 'Go back';
  goBackButtonColor: string = 'blue';

  //Flag used to track if the user has made any changes to the form
  hasChanges: boolean = false;

  //Variables for warning box when there are changes

  //Variables for warning box buttons for discarding changes
  discardChangesButtonText: string = 'Yes, discard changes';
  discardChangesButtonColor: string = 'red';

  //Variables for warning box buttons for keeping editing
  keepEditingButtonText: string = 'No, keep editing';
  keepEditingButtonColor: string = 'blue';

  ngOnInit(): void {
    console.log('Selected education log: ', this.educationLog);
    console.log('Is create or edit mode: ', this.isCreateOrEditMode);
    if (this.educationLog) {
      console.log('Edit mode');
      this.isCreateOrEditMode = false;
      //Set the initial values of the form controls to the values of the selected education log
      this.educationInformationFormControl.patchValue({
        institutionName: this.educationLog.institutionName,
        institutionOfficialWebsite:
          this.educationLog.institutionOfficialWebsite,
        institutionAddress: this.educationLog.institutionAddress,
        educationLevel: this.educationLog.educationLevel,
        fieldOfStudy: this.educationLog.fieldOfStudy,
        minorFieldOfStudy: this.educationLog.minorFieldOfStudy,
        startDate: this.educationLog.startDate,
        endDate: this.educationLog.endDate,
        isCompletedGroup: {
          firstOption: this.educationLog.isCompleted,
          secondOption: !this.educationLog.isCompleted,
        },
        finalGrade: this.educationLog.finalGrade,
        description: this.educationLog.description,
      });
      console.log('Initial values: ', this.initialValues);
      console.log(
        this.educationInformationFormControl.controls.institutionOfficialWebsite
          .value
      );
    }
    if (!this.educationLog) {
      this.isCreateOrEditMode = true;
    }
    this.initialValues = this.educationInformationFormControl.getRawValue();

    //Track changes in the form controls
    this.educationInformationFormControl.valueChanges.subscribe((changes) => {
      this.changedControls = Object.keys(
        this.educationInformationFormControl.value
      ).filter((key) => {
        if (
          this.initialValues[key as keyof typeof this.initialValues] !==
          changes[key as keyof typeof changes]
        ) {
          this.hasChanges = true;
        }
      });

      if (this.educationInformationFormControl.valid) {
        this.formErrorMessage = 'You can save your data';
      }
      if (this.educationInformationFormControl.invalid) {
        this.formErrorMessage = 'Please resolve the errors before saving';
      }
    });

    //Track changes for the education level field
    this.educationInformationFormControl.controls.educationLevel.valueChanges.subscribe(
      (change) => {
        this.trackEducationLevelErrors();
      }
    );
  }

  checkControlValueChanges() {}
  onDiscardData() {
    this.currentDataOperation = false;
    console.log(this.initialValues);
    console.log(this.educationInformationFormControl.value);

    console.log(this.changedControls);

    if (this.changedControls.length > 0) {
      console.log('Data has been changed');
      this.warningBoxTitle = 'You have unsaved changes';
      this.warningBoxMessage =
        'You have unsaved changes. Do you want to discard them?';
      this.warningBoxYesButtonText = "No, don't discard";
      this.warningBoxYesButtonColor = 'green';
      this.warningBoxNoButtonText = 'Yes, discard data';
      this.warningBoxNoButtonColor = 'red';

      this.toggleWarningAnnouncementBox(new Event('click'));
    }
    if (this.changedControls.length === 0) {
      console.log('Data has not been changed');
      this.toggleFloatingBox();
    }
  }

  onSaveData() {
    this.currentDataOperation = true;

    console.log(this.educationInformationFormControl.value);
    //Check if the form is valid
    if (this.educationInformationFormControl.invalid) {
      this.trackEducationLevelErrors();
      this.trackFieldOfStudyErrors();
      this.trackIsCompletedErrors();
      this.formErrorMessage = 'Please resolve any errors before saving';
    }
    //Check if the form is valid
    if (this.educationInformationFormControl.valid) {
      this.formErrorMessage = 'You can save your data';
      this.warningBoxTitle = 'Do you want to save your data?';
      this.warningBoxMessage = 'Are you sure you want to save your data?';
      this.warningBoxYesButtonText = 'Yes, save';
      this.warningBoxYesButtonColor = 'green';
      this.warningBoxNoButtonText = 'No, keep editing';
      this.warningBoxNoButtonColor = 'red';
      this.toggleWarningAnnouncementBox(new Event('click'));
    }
  }
  onKeepEditing() {
    this.toggleWarningAnnouncementBox(new Event('click'));
  }
  toggleWarningAnnouncementBox($event: Event) {
    this.showWarningBox = !this.showWarningBox;
  }
  onYesDiscardData() {
    this.showWarningBox = false;
    this.toggleFloatingBox();
  }

  trackEducationLevelErrors() {
    //Store all the errors
    const error =
      this.educationInformationFormControl.controls.educationLevel.errors;

    //Check if the errors exist
    if (error) {
      if (error['required']) {
        this.educationLevelError = 'This field is required';
      }
      if (error['pattern']) {
        this.educationLevelError = 'Only letters and numbers are allowed';
      }
    }

    //Check if there are no errors, and remove the error message
    if (!error) {
      this.educationLevelError = '';
    }
  }

  trackFieldOfStudyErrors() {
    //Store all the errors
    const error =
      this.educationInformationFormControl.controls.fieldOfStudy.errors;

    //Check if the errors exist
    if (error) {
      if (error['required']) {
        this.fieldOfStudyError = 'This field is required';
      }
      if (error['pattern']) {
        this.fieldOfStudyError = 'Only letters and numbers are allowed';
      }
    }

    //Check if there are no errors, and remove the error message
    if (!error) {
      this.fieldOfStudyError = '';
    }
  }

  trackIsCompletedErrors() {}

  //
  keepEditing() {
    console.log('Keep editing');
    this.toggleWarningAnnouncementBox(new Event('click'));
  }

  discardData() {
    console.log('Discard data');
    this.toggleWarningAnnouncementBox(new Event('click'));
    this.toggleFloatingBox();
  }
  saveData() {
    console.log('Save data');
    const newEducationInformation: EducationInformationHttpRequest = {
      institutionName: this.educationInformationFormControl.value
        .institutionName as string,
      institutionOfficialWebsite: this.educationInformationFormControl.value
        .institutionOfficialWebsite as string,
      institutionAddress: this.educationInformationFormControl.value
        .institutionAddress as string,
      educationLevel: this.educationInformationFormControl.value
        .educationLevel as string,
      fieldOfStudy: this.educationInformationFormControl.value
        .fieldOfStudy as string,
      minorFieldOfStudy: this.educationInformationFormControl.value
        .minorFieldOfStudy as string,
      startDate: DateHelper.toDateOnlyString(new Date()),
      endDate: DateHelper.toDateOnlyString(new Date()),
      isCompleted: true,
      finalGrade: this.educationInformationFormControl.value
        .finalGrade as string,
      description: this.educationInformationFormControl.value
        .description as string,
    };

    this.warningBoxMessage = 'Saving education data...';
    this.warningBoxMessageColor = 'orange';
    this.isDataTransmissionActive = true;

    let saveResult = null;
    this.accountService
      .createPersonEducationInformation(newEducationInformation)
      .then((result) => {
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;
        console.log('Result: ', result);
        saveResult = result;
        if ((result?.success as string) === 'true') {
          this.warningBoxMessage = 'Data saved successfully';
          this.warningBoxMessageColor = 'green';
        }
        if ((result?.success as string) === 'false') {
          this.warningBoxMessage = result?.message as string;
          if (
            result?.message ===
            'Cannot add more than 5 education information per account'
          ) {
            this.isResponseMaxLengthReachedError = true;
          }
          this.warningBoxMessageColor = 'red';
        }
        this.saveOperationResult = result?.message as string;
      });
    console.log('Save result: ', saveResult);
  }

  onConfirmButton() {
    console.log('Confirm button clicked');
    console.log('Current data operation: ', this.currentDataOperation);

    if (this.currentDataOperation) {
      this.saveData();
    }
    if (!this.currentDataOperation) {
      this.keepEditing();
    }
  }

  onCancelButton() {
    console.log('Cancel button clicked');
    console.log('Current data operation: ', this.currentDataOperation);
    if (this.currentDataOperation) {
      this.keepEditing();
    }
    if (!this.currentDataOperation) {
      this.discardData();
    }
  }

  handleContinueButton() {
    this.toggleWarningAnnouncementBox(new Event('click'));
    this.toggleFloatingBox();
  }

  handleEditButton() {
    this.toggleWarningAnnouncementBox(new Event('click'));
  }

  onDiscardChanges() {
    console.log('Discard changes');
    this.warningBoxTitle = 'Are you sure you want to discard changes?';
    this.warningBoxMessage = 'Changes you have made will be discarded';
    this.warningBoxMessageColor = 'red';
    this.currentDataOperation = false;
    this.toggleWarningAnnouncementBox(new Event('click'));
  }
  onSaveChanges() {
    console.log('Save changes');
  }
  onGoBack() {
    console.log('Go back');
  }
}
