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
  @Input() toggleFloatingBox: () => void = () => {};
  //Education information object that is passed from the parent component, if it is null, then it is a new education information object
  @Input() educationInformation: EducationInformation | null = null;
  referenceEducationInformation: EducationInformation | null = null;
  @Input() isEditModalOpen: boolean = true;

  //Flag to determine if the education information object have data added to it, or data updated
  //True - Create mode
  //False - Edit mode
  isCreateOrEditMode: boolean = false;

  //FormGroup for education information
  educationInformationFormGroup = new FormGroup({
    institutionName: new FormControl(),
    institutionOfficialWebsite: new FormControl(),
    institutionAddress: new FormControl(),
    educationLevel: new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      Validators.pattern(/^(?=.*\p{L})[\p{L}\p{N}.]+(?: [\p{L}\p{N}.]+)*$/u),
    ]),
    fieldOfStudy: new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      Validators.pattern(/^(?=.*\p{L})[\p{L}\p{N}.]+(?: [\p{L}\p{N}.]+)*$/u),
    ]),
    minorFieldOfStudy: new FormControl(),
    startDate: new FormControl(),
    endDate: new FormControl(),
    isCompletedFormGroup: new FormGroup({
      firstOption: new FormControl(false),
      secondOption: new FormControl(true),
    }),
    finalGrade: new FormControl(),
    description: new FormControl(),
  });

  //Variable for error message that is displayed when the form is invalid, below the form
  formErrorMessage: string = '';
  formErrorMessageColor: string = 'red';

  hasChanges: boolean = false;

  //Variables for the input component fields

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

  //Variables for submit button
  submitButtonText: string = 'Save data';
  submitButtonColor: string = 'green';

  //Variables for discard button
  discardButtonText: string = 'Discard data';
  discardButtonColor: string = 'red';

  //Variables for go back button
  goBackButtonText: string = 'Go back';
  goBackButtonColor: string = 'blue';

  //Record of the initial values when the component is loaded
  initialValues: Record<string, any> = {};

  //Floating warning box used to control the operations with the data
  floatingWarningBoxTitle: string = 'Warning';
  floatingWarningBoxMessage: string = '';
  floatingWarningBoxMessageColor: string = 'red';
  showWarningBox: boolean = false;
  confirmButtonText: string = 'Confirm';
  confirmButtonColor: string = 'Green';
  cancelButtonText: string = 'Cancel';
  cancelButtonColor: string = 'Red';
  warningBoxButtonMargin: string = '12px 0px';

  //Flag to specify which operation is being performed
  //True - save data
  //False - discard data
  currentDataOperation: boolean = true;

  isInstitutionNameChanged: boolean = false;
  isInstitutionOfficialWebsiteChanged: boolean = false;
  isInstitutionAddressChanged: boolean = false;
  isEducationLevelChanged: boolean = false;
  isFieldOfStudyChanged: boolean = false;
  isMinorFieldOfStudyChanged: boolean = false;
  isStartDateChanged: boolean = false;
  isEndDateChanged: boolean = false;
  isIsCompletedChanged: boolean = false;
  isFinalGradeChanged: boolean = false;
  isDescriptionChanged: boolean = false;

  ngOnInit(): void {
    console.log('Education information: ', this.educationInformation);
    //Set the component mode
    if (
      this.educationInformation?.educationLevel !== '' &&
      this.educationInformation?.fieldOfStudy !== '' &&
      this.educationInformation
    ) {
      this.isCreateOrEditMode = false;
      //Set the initial values of the form controls to the values of the selected education log
      this.educationInformationFormGroup.patchValue({
        institutionName: this.educationInformation.institutionName,
        institutionOfficialWebsite:
          this.educationInformation.institutionOfficialWebsite,
        institutionAddress: this.educationInformation.institutionAddress,
        educationLevel: this.educationInformation.educationLevel,
        fieldOfStudy: this.educationInformation.fieldOfStudy,
        minorFieldOfStudy: this.educationInformation.minorFieldOfStudy,
        startDate: this.educationInformation.startDate,
        endDate: this.educationInformation.endDate,
        isCompletedFormGroup: {
          firstOption: this.educationInformation.isCompleted,
          secondOption: !this.educationInformation.isCompleted,
        },
        finalGrade: this.educationInformation.finalGrade,
        description: this.educationInformation.description,
      });
    }
    if (
      this.educationInformation?.educationLevel === '' &&
      this.educationInformation?.fieldOfStudy === ''
    ) {
      this.isCreateOrEditMode = true;
    }
    this.initialValues = this.educationInformationFormGroup.getRawValue();
    console.log('Is create or edit mode: ', this.isCreateOrEditMode);
    //Set the discard and submit button, (and go back button) visibility, and other options
    this.setSubmitButtonOptions();
    this.setDiscardButtonOptions();

    console.log('Passed education information: ', this.educationInformation);
    console.log('Form group values:', this.educationInformationFormGroup.value);
    console.log('Initial values: ', this.initialValues);
    this.referenceEducationInformation = this.educationInformation;

    this.educationInformationFormGroup.valueChanges.subscribe((changes) => {
      console.log(
        'Form values changed:',
        this.educationInformationFormGroup.value
      );
      if (
        this.educationInformationFormGroup.controls.institutionName.value === ''
      ) {
        this.educationInformationFormGroup.controls.institutionName.setValue(
          null
        );
      }
      if (
        this.educationInformationFormGroup.controls.institutionOfficialWebsite
          .value === ''
      ) {
        this.educationInformationFormGroup.controls.institutionOfficialWebsite.setValue(
          null,
          {
            emitEvent: false,
          }
        );
      }
      if (
        this.educationInformationFormGroup.controls.institutionAddress.value ===
        ''
      ) {
        this.educationInformationFormGroup.controls.institutionAddress.setValue(
          null,
          {
            emitEvent: false,
          }
        );
      }
      if (
        this.educationInformationFormGroup.controls.minorFieldOfStudy.value ===
        ''
      ) {
        this.educationInformationFormGroup.controls.minorFieldOfStudy.setValue(
          null,
          {
            emitEvent: false,
          }
        );
      }
      if (
        typeof this.educationInformationFormGroup.controls.startDate.value ===
          'object' ||
        (typeof this.educationInformationFormGroup.controls.startDate.value ===
          'string' &&
          this.educationInformationFormGroup.controls.startDate.value === '')
      ) {
        this.educationInformationFormGroup.controls.startDate.setValue(null, {
          emitEvent: false,
        });
      }
      if (
        typeof this.educationInformationFormGroup.controls.endDate.value ===
          'object' ||
        (typeof this.educationInformationFormGroup.controls.endDate.value ===
          'string' &&
          this.educationInformationFormGroup.controls.endDate.value === '')
      ) {
        this.educationInformationFormGroup.controls.endDate.setValue(null, {
          emitEvent: false,
        });
      }

      if (this.educationInformationFormGroup.controls.finalGrade.value === '') {
        this.educationInformationFormGroup.controls.finalGrade.setValue(null, {
          emitEvent: false,
        });
      }
      if (
        this.educationInformationFormGroup.controls.description.value === ''
      ) {
        this.educationInformationFormGroup.controls.description.setValue(null, {
          emitEvent: false,
        });
      }
      console.log(
        'Final grade value: ',
        this.educationInformationFormGroup.controls.finalGrade.value
      );
      console.log(
        'Initial value for isCompleted firstOption: ',
        this.initialValues['isCompletedFormGroup'].firstOption
      );
      this.checkForChanges();
      if (this.educationInformationFormGroup.invalid) {
        this.formErrorMessage = 'Please resolve the errors, to continue';
        this.formErrorMessageColor = 'red';
      }
      if (this.educationInformationFormGroup.valid) {
        this.formErrorMessage = '';
        this.formErrorMessageColor = '';
      }
      console.log('Has changes: ', this.hasChanges);
    });

    //Track changes in the form controls
    //Check for the errors in the education level field
    this.educationInformationFormGroup.controls.educationLevel.valueChanges.subscribe(
      () => {
        this.checkEducationLevel();
      }
    );

    //Check for the errors in the field of study field
    this.educationInformationFormGroup.controls.fieldOfStudy.valueChanges.subscribe(
      () => {
        this.checkFieldOfStudy();
      }
    );
  }

  //Sets the submit button options
  //If the component is in create mode, the submit button will have following options: color - green, text - Save data
  //If the component is in edit mode, the submit button will have following options: color - green, text - Save changes
  setSubmitButtonOptions() {
    if (this.isCreateOrEditMode) {
      this.submitButtonText = 'Save data';
      this.submitButtonColor = 'green';
    }
    if (!this.isCreateOrEditMode) {
      this.submitButtonText = 'Save changes';
      this.submitButtonColor = 'green';
    }
  }
  //Sets the discard button options
  //If the component is in create mode, the discard button will have following options: color - red, text - Discard data
  //If the component is in edit mode, the discard button will have following options: color - red, text - Discard changes
  setDiscardButtonOptions() {
    if (this.isCreateOrEditMode) {
      this.discardButtonText = 'Discard data';
      this.discardButtonColor = 'red';
    }
    if (!this.isCreateOrEditMode) {
      this.discardButtonText = 'Discard changes';
      this.discardButtonColor = 'red';
    }
  }

  //Check if the education level field is valid
  checkEducationLevel() {
    console.log(
      'Education level error:',
      this.educationInformationFormGroup.controls.educationLevel.errors
    );
    const educationLevelError =
      this.educationInformationFormGroup.controls.educationLevel.errors;

    if (educationLevelError) {
      if (educationLevelError['required']) {
        this.educationLevelError = 'This field is required';
      }
      if (educationLevelError['minlength']) {
        this.educationLevelError =
          'This field must be at least 2 characters long';
      }
      if (educationLevelError['maxlength']) {
        this.educationLevelError =
          'This field can only be up to 100 characters long';
      }
      if (educationLevelError['pattern']) {
        this.educationLevelError =
          'This field can only contain letters, numbers, and spaces but between other characters';
      }
    }
    if (!educationLevelError) {
      this.educationLevelError = '';
    }
  }

  //Check if the field of study field is valid
  checkFieldOfStudy() {
    console.log(
      'Field of study error:',
      this.educationInformationFormGroup.controls.fieldOfStudy.errors
    );
    const fieldOfStudyError =
      this.educationInformationFormGroup.controls.fieldOfStudy.errors;

    if (fieldOfStudyError) {
      if (fieldOfStudyError['required']) {
        this.fieldOfStudyError = 'This field is required';
      }
      if (fieldOfStudyError['minlength']) {
        this.fieldOfStudyError =
          'This field must be at least 2 characters long';
      }
      if (fieldOfStudyError['maxlength']) {
        this.fieldOfStudyError =
          'This field can only be up to 100 characters long';
      }
      if (fieldOfStudyError['pattern']) {
        this.fieldOfStudyError =
          'This field can only contain letters, numbers, and spaces but between other characters';
      }
    }
    if (!fieldOfStudyError) {
      this.fieldOfStudyError = '';
    }
  }

  checkForChanges() {
    if (
      this.educationInformationFormGroup.controls.institutionName.value !==
      this.initialValues['institutionName']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.institutionOfficialWebsite
        .value !== this.initialValues['institutionOfficialWebsite']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.institutionAddress.value !==
      this.initialValues['institutionAddress']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.educationLevel.value !==
      this.initialValues['educationLevel']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.fieldOfStudy.value !==
      this.initialValues['fieldOfStudy']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.minorFieldOfStudy.value !==
      this.initialValues['minorFieldOfStudy']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.startDate.value !==
      this.initialValues['startDate']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.endDate.value !==
      this.initialValues['endDate']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.isCompletedFormGroup.controls
        .firstOption.value !==
      this.initialValues['isCompletedFormGroup'].firstOption
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.finalGrade.value !==
      this.initialValues['finalGrade']
    ) {
      this.hasChanges = true;
      return;
    }
    if (
      this.educationInformationFormGroup.controls.description.value !==
      this.initialValues['description']
    ) {
      this.hasChanges = true;
      return;
    }
    this.hasChanges = false;
  }

  toggleWarningBox() {
    console.log('Show warning box');
    this.showWarningBox = !this.showWarningBox;
  }
  onGoBack() {
    this.toggleFloatingBox();
  }

  onDiscardButtonClick() {
    this.toggleWarningBox();
    this.currentDataOperation = false;
    if (this.isCreateOrEditMode) {
      this.floatingWarningBoxTitle = 'Discard date';
      this.floatingWarningBoxMessage =
        'All data you have added will be lost, are you sure you want to discard?';
      this.floatingWarningBoxMessageColor = 'red';
      this.confirmButtonColor = 'red';
      this.confirmButtonText = 'Yes, discard';
      this.cancelButtonText = 'No, keep editing';
      this.cancelButtonColor = 'blue';
    }
    if (!this.isCreateOrEditMode) {
      this.floatingWarningBoxTitle = 'Discard Changes';
      this.floatingWarningBoxMessage =
        'All changes will be lost, are you sure you want to discard?';
      this.floatingWarningBoxMessageColor = 'red';
      this.confirmButtonColor = 'red';
      this.confirmButtonText = 'Yes, discard';
      this.cancelButtonText = 'No, keep editing';
      this.cancelButtonColor = 'blue';
    }
  }

  onSaveButtonClick() {
    if (this.educationInformationFormGroup.invalid) {
      this.formErrorMessage = 'Please resolve any errors before saving';
    }
    if (this.educationInformationFormGroup.valid) {
      console.log('Save button');
      this.currentDataOperation = true;
      this.toggleWarningBox();
      if (this.isCreateOrEditMode) {
        this.floatingWarningBoxTitle = 'Save data';
        this.floatingWarningBoxMessage =
          'Data you have added will be saved, are you sure you want to save?';
        this.floatingWarningBoxMessageColor = 'green';
        this.confirmButtonColor = 'green';
        this.confirmButtonText = 'Yes, save';
        this.cancelButtonText = 'No, keep editing';
        this.cancelButtonColor = 'blue';
      }
      if (!this.isCreateOrEditMode) {
        this.floatingWarningBoxTitle = 'Update changes';
        this.floatingWarningBoxMessage =
          'Changes you have made will be saved, are you sure you want to save?';
        this.floatingWarningBoxMessageColor = 'green';
        this.confirmButtonColor = 'green';
        this.confirmButtonText = 'Yes, save changes';
        this.cancelButtonText = 'No, keep editing';
      }
      if (this.isCreateOrEditMode) {
        this.floatingWarningBoxTitle = 'Save data';
        this.floatingWarningBoxMessage =
          'Data you have added will be saved, are you sure you want to save?';
        this.floatingWarningBoxMessageColor = 'green';
        this.confirmButtonColor = 'green';
        this.confirmButtonText = 'Yes, save';
        this.cancelButtonText = 'No, keep editing';
        this.cancelButtonColor = 'blue';
      }
      if (!this.isCreateOrEditMode) {
        this.floatingWarningBoxTitle = 'Update changes';
        this.floatingWarningBoxMessage =
          'Changes you have made will be saved, are you sure you want to save?';
        this.floatingWarningBoxMessageColor = 'green';
        this.confirmButtonColor = 'green';
        this.confirmButtonText = 'Yes, save changes';
        this.cancelButtonText = 'No, keep editing';
        this.cancelButtonColor = 'blue';
        this.cancelButtonColor = 'blue';
      }
    }
  }

  onCancelButtonClick() {
    this.toggleWarningBox();
  }

  onConfirmButtonClick() {
    if (!this.currentDataOperation) {
      this.toggleWarningBox();
      this.toggleFloatingBox();
    }
    if (this.currentDataOperation) {
      if (this.isCreateOrEditMode) {
        this.createEducationInformation();
      }
      if (!this.isCreateOrEditMode) {
        this.updateEducationInformation();
      }
    }
  }

  createEducationInformation() {
    console.log('Saving data...');
  }

  updateEducationInformation() {
    console.log('Updating data...');
  }
}
