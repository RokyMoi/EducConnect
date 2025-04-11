import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';

import {
  FormGroup,
  ReactiveFormsModule,
  FormControl,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';

import { forkJoin, tap } from 'rxjs';

import { ErrorStateMatcher } from '@angular/material/core';

import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { NgIf } from '@angular/common';

import CareerInformation from '../../../../../models/person/career/careerInformation';
import { AccountService } from '../../../../../services/account.service';
import { TextInputComponentComponent } from '../../../../../common/input/text/text-input-component/text-input-component.component';
import { SelectDropdownComponent } from '../../../../../common/select/select-dropdown/select-dropdown.component';
import { ReferenceService } from '../../../../../services/reference/reference.service';
import EmploymentType from '../../../../../models/reference/employmentType/employmentType';
import { DatePickerComponent } from '../../../../../common/input/date/date-picker/date-picker/date-picker.component';
import IndustryClassification from '../../../../../models/reference/industryClassification/industryClassification';
import WorkType from '../../../../../models/reference/workType/workType';
import { SubmitButtonComponent } from '../../../../../common/button/submit-button/submit-button.component';
import DateHelper from '../../../../../helpers/date.helper';
import CareerInformationHttpSaveRequest from '../../../../../models/person/career/careerInformationHttpSaveRequest';
import { FloatingWarningBoxComponent } from '../../../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import EducationInformationHttpUpdateRequest from '../../../../../models/person/education/EducationInformationHttpUpdateRequest';
import CareerInformationHttpUpdateRequest from '../../../../../models/person/career/careerInformationHttpUpdateRequest';

@Component({
  standalone: true,
  selector: 'app-career-signup-log',
  imports: [
    ReactiveFormsModule,
    TextInputComponentComponent,
    SelectDropdownComponent,
    NgIf,
    DatePickerComponent,
    SubmitButtonComponent,
    FloatingWarningBoxComponent,
    MatProgressSpinner,
  ],
  templateUrl: './career-signup-log.component.html',
  styleUrl: './career-signup-log.component.css',
})
export class CareerSignupLogComponent implements OnInit {
  //Career information object that is passed from the parent component, if it is null, then it is a new career information object
  @Input() careerInformation: CareerInformation | null = null;
  @Input() isEditModalOpen: boolean = true;

  @Output() closeCareerInformationLog = new EventEmitter<void>();

  accountService: AccountService = inject(AccountService);
  referenceService: ReferenceService = inject(ReferenceService);

  employmentTypes: EmploymentType[] = [];
  employmentTypeOptions: any[] = [];

  industryClassifications: IndustryClassification[] = [];
  industryClassificationOptions: any[] = [];

  workTypes: WorkType[] = [];
  workTypeOptions: any[] = [];

  isWarningDialogOpen: boolean = false;

  //Record of the initial values when the component is loaded
  initialValues: Record<string, any> = {};

  //Text to display once the data has been retrieved from the server
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';
  isDataTransmissionActive: boolean = false;
  isDataTransmissionComplete: boolean = false;
  continueButtonText: string = 'View all career information';
  continueButtonColor: string = 'blue';

  //Array that holds the updated values and their form control names
  changedValues: Record<string, any> = {};

  ngOnInit(): void {
    //Set the component mode
    console.log('Career information:', this.careerInformation);
    this.isCreateOrEditMode = this.careerInformation ? false : true;
    this.isCreateOrUpdateOperation = this.isCreateOrEditMode;

    this.setSaveButtonValues();
    this.setDiscardButtonValues();
    if (this.isEditModalOpen) {
      this.fetchAllData().subscribe(() => {
        if (this.careerInformation) {
          const employmentTypeName = this.getEmploymentTypeName(
            this.careerInformation.employmentTypeId
          );
          this.careerInformationFormGroup.patchValue({
            companyName: this.careerInformation.companyName,
            companyWebsite: this.careerInformation.companyWebsite,
            jobTitle: this.careerInformation.jobTitle,
            position: this.careerInformation.position,
            cityOfEmployment: this.careerInformation.cityOfEmployment,
            countryOfEmployment: this.careerInformation.countryOfEmployment,
            employmentType: this.careerInformation.employmentTypeId.toString(),
            startDate: this.careerInformation.startDate.toString(),
            endDate: this.careerInformation.endDate
              ? this.careerInformation.endDate.toString()
              : null,
            jobDescription: this.careerInformation.jobDescription,
            responsibilities: this.careerInformation.responsibilities,
            achievements: this.careerInformation.achievements,
            industry:
              this.careerInformation.industryClassificationId.toString(),
            skillsUsed: this.careerInformation.skillsUsed,
            workTypeId: this.careerInformation.workTypeId
              ? this.careerInformation.workTypeId.toString()
              : '',
            additionalInformation: this.careerInformation.additionalInformation,
          });
          this.hasChanges = false;
          this.changedValues = {};
        }
        this.initialValues = {
          ...this.careerInformationFormGroup.value,
        };
      });
    }

    this.careerInformationFormGroup.controls.companyName.valueChanges.subscribe(
      () => {
        this.checkCompanyName();
      }
    );
    this.careerInformationFormGroup.controls.companyWebsite.valueChanges.subscribe(
      () => {
        this.checkCompanyWebsite();
      }
    );
    this.careerInformationFormGroup.controls.jobTitle.valueChanges.subscribe(
      () => {
        const errors = this.careerInformationFormGroup.controls.jobTitle.errors;
        this.checkJobTitle();
        console.log('Errors: ', errors);
      }
    );
    this,
      this.careerInformationFormGroup.controls.position.valueChanges.subscribe(
        () => {
          this.checkPosition();
        }
      );
    this.careerInformationFormGroup.controls.cityOfEmployment.valueChanges.subscribe(
      () => {
        this.checkCityOfEmployment();
      }
    );
    this.careerInformationFormGroup.controls.countryOfEmployment.valueChanges.subscribe(
      () => {
        this.checkCountryOfEmployment();
      }
    );
    this.careerInformationFormGroup.controls.employmentType.valueChanges.subscribe(
      () => {
        this.checkEmploymentType();
      }
    );
    this.careerInformationFormGroup.controls.startDate.valueChanges.subscribe(
      () => {
        this.checkStartDate();
      }
    );
    this.careerInformationFormGroup.controls.jobDescription.valueChanges.subscribe(
      () => {
        this.checkJobDescription();
      }
    );
    this.careerInformationFormGroup.controls.responsibilities.valueChanges.subscribe(
      () => {
        this.checkResponsibilities();
      }
    );
    this.careerInformationFormGroup.controls.achievements.valueChanges.subscribe(
      () => {
        this.checkAchievements();
      }
    );
    this.careerInformationFormGroup.controls.industry.valueChanges.subscribe(
      () => {
        this.checkIndustry();
      }
    );
    this.careerInformationFormGroup.controls.skillsUsed.valueChanges.subscribe(
      () => {
        this.checkSkillsUsed();
      }
    );
    this.careerInformationFormGroup.controls.additionalInformation.valueChanges.subscribe(
      () => {
        this.checkAdditionalInformation();
      }
    );
    this.careerInformationFormGroup.valueChanges.subscribe((values) => {
      this.checkForChanges();

      if (!this.hasChanges) {
        this.changedValues = {};
        console.log('Values cleared, ', this.changedValues);
      }

      Object.keys(values).forEach((key) => {
        const control = this.careerInformationFormGroup.get(key);
        if (key === 'endDate') {
          console.log(
            'End date changed: ',
            this.careerInformationFormGroup.controls.endDate.value
          );
          if (this.careerInformationFormGroup.controls.endDate.value === null) {
            delete this.changedValues[key];
            console.log('End date after deletion: ', this.changedValues);
          }
        }
        if (control?.value !== this.initialValues[key]) {
          console.log('Value change assigned: ', key);
          console.log('Value change control:', control);
          this.changedValues[key] = control?.value;
        }
      });
      console.log('Changed values: ', this.changedValues);
      console.log('Values changed: ', this.hasChanges);

      if (this.isSaveButtonClicked) {
        if (this.careerInformationFormGroup.invalid) {
          this.formErrorMessage = 'Please resolve any errors before saving.';
          this.formErrorMessageColor = 'red';
        }
        if (this.careerInformationFormGroup.valid) {
          this.formErrorMessage = 'All fields are valid.';
          this.formErrorMessageColor = 'green';
        }
      }
    });
  }

  //Flag to determine if the career information object should have data added to it, or data updated
  //True - Create mode
  //False - Edit mode
  isCreateOrEditMode: boolean = false;

  //Flag to determine if the component log values have any new changes to update to
  //True - has changes
  //False - no changes
  hasChanges: boolean = false;

  //Flag to determine which operation to proceed with
  //True - Create new career information record
  //False - Update career information record
  isCreateOrUpdateOperation: boolean = true;

  //Flag to determine if the selected operation is delete or not
  //True - Delete operation
  //False - Not a delete operation
  isDeleteOperation: boolean = false;

  //Flag to determine if the save button was clicked
  //True - Save button was clicked
  //False - Save button was not clicked
  isSaveButtonClicked: boolean = false;

  //Flag to determine if the discard button was clicked
  //True - Discard button was clicked
  //False - Discard button was not clicked
  isDiscardButtonClicked: boolean = false;

  //Variables for buttons
  goBackButtonColor: string = 'blue';
  goBackButtonText: string = 'Go Back';

  //Variables for the delete record button
  deleteButtonColor: string = 'red';
  deleteButtonText: string = 'Delete this item';

  //Variables for the save button
  saveButtonColor: string = 'green';
  saveButtonText: string = 'Save';

  //Variables for the discard button
  discardButtonColor: string = 'red';
  discardButtonText: string = 'Discard';

  //Variables for the confirm button
  confirmButtonColor: string = 'green';
  confirmButtonText: string = 'Confirm';

  //Variables for the cancel button
  cancelButtonColor: string = 'blue';
  cancelButtonText: string = 'No, keep editing';

  //Variables for the go back button
  goBackToCareerInformationButtonColor: string = 'blue';
  goBackToCareerInformationButtonText: string = 'Go back';
  //Common button variables
  warningBoxButtonMargin: string = '12px 0px';
  //FormGroup for the career information
  careerInformationFormGroup = new FormGroup({
    companyName: new FormControl('', [
      Validators.required,
      Validators.pattern(/^(?!.*(?:^\s.*\S|\S.*\s|\s.*\s)$).*/u),
    ]),
    companyWebsite: new FormControl('', [
      this.optionalParameterRegExValidator(
        /^(https?:\/\/)?([\w\-]+\.)+[a-z]{2,}(\/[\w\-._~:\/?#[\]@!$&'()*+,;=]*)?$/
      ),
    ]),
    jobTitle: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[^\s].*[^\s]$|^[^\s]$/u),
    ]),
    position: new FormControl('', [
      this.optionalParameterRegExValidator(
        /^(?!.*(?:^\s.*\S|\S.*\s|\s.*\s)$).*/u
      ),
    ]),
    cityOfEmployment: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[^\s].*[^\s]$|^[^\s]$/u),
    ]),
    countryOfEmployment: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[^\s].*[^\s]$|^[^\s]$/u),
    ]),
    employmentType: new FormControl('', [Validators.required]),
    startDate: new FormControl('', [Validators.required]),
    endDate: new FormControl(),
    jobDescription: new FormControl('', [
      this.optionalParameterRegExValidator(
        /^(?!.*(?:^\s.*\S|\S.*\s|\s.*\s)$).*/u
      ),
    ]),
    responsibilities: new FormControl('', [
      this.optionalParameterRegExValidator(
        /^(?!.*(?:^\s.*\S|\S.*\s|\s.*\s)$).*/u
      ),
    ]),
    achievements: new FormControl('', [
      this.optionalParameterRegExValidator(
        /^(?!.*(?:^\s.*\S|\S.*\s|\s.*\s)$).*/u
      ),
    ]),
    industry: new FormControl('', [Validators.required]),
    skillsUsed: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[^\s].*[^\s]$|^[^\s]$/u),
    ]),
    workTypeId: new FormControl(''),
    additionalInformation: new FormControl('', [
      this.optionalParameterRegExValidator(
        /^(?!.*(?:^\s.*\S|\S.*\s|\s.*\s)$).*/u
      ),
    ]),
  });

  //Variables for companyName
  companyNameLabel: string = 'Company Name';
  companyNameErrorMessage: string = '';
  companyNamePlaceholder: string =
    'Enter the official name of the company you worked for';

  //Variables for companyWebsite
  companyWebsiteLabel: string = 'Official website of the company';
  companyWebsiteErrorMessage: string = '';
  companyWebsitePlaceholder: string =
    'Enter the official website of the company you worked for';

  //Variables for jobTitle
  jobTitleLabel: string = 'Title you held at the company';
  jobTitleErrorMessage: string = '';
  jobTitlePlaceholder: string =
    'Enter the title you held at the company (eg. Tea Lead, Senior Developer, etc.)';

  //Variables for position
  positionLabel: string = 'Position';
  positionErrorMessage: string = '';
  positionPlaceholder: string =
    'Enter the position you held (eg. Manager, Supervisor, etc.)';

  //Variables for cityOfEmployment
  cityOfEmploymentLabel: string = 'City of Employment';
  cityOfEmploymentErrorMessage: string = '';
  cityOfEmploymentPlaceholder: string = 'Enter the city you worked at';

  //Variables for countryOfEmployment
  countryOfEmploymentLabel: string = 'Country of Employment';
  countryOfEmploymentErrorMessage: string = '';
  countryOfEmploymentPlaceholder: string = 'Enter the country you worked in';

  //Variables for employmentType
  employmentTypeLabel: string = 'Employment Type';
  employmentTypeErrorMessage: string = '';
  employmentTypePlaceholder: string = 'Select the type of employment';

  //Variables for startDate
  startDateLabel: string = 'When did you join the company?';
  startDateErrorMessage: string = '';
  startDatePlaceholder: string = 'Enter the start date';

  //Variables for endData
  endDateLabel: string = 'When did you leave the company?';
  endDateErrorMessage: string = '';
  endDatePlaceholder: string = 'Enter the end date';

  //Variables for jobDescription
  jobDescriptionLabel: string = 'Job Description';
  jobDescriptionErrorMessage: string = '';
  jobDescriptionPlaceholder: string = 'Describe what your job was like';

  //Variables for responsibilities
  responsibilitiesLabel: string = 'Responsibilities';
  responsibilitiesErrorMessage: string = '';
  responsibilitiesPlaceholder: string =
    'Describe the responsibilities you had during your tenure at the company';

  //Variables for achievements
  achievementsLabel: string = 'Achievements';
  achievementsErrorMessage: string = '';
  achievementsPlaceholder: string =
    'Describe the achievements you had achieved while working at the company';

  //Variables for industry
  industryLabel: string = 'Industry';
  industryErrorMessage: string = '';
  industryPlaceholder: string = 'Select the industry you worked in';

  //Variables for skillsUsed
  skillsUsedLabel: string = 'Skills Used';
  skillsUsedErrorMessage: string = '';
  skillsUsedPlaceholder: string = 'Enter the skills you used at the company';

  //Variables for workType
  workTypeLabel: string = 'Work Type';
  workTypeErrorMessage: string = '';
  workTypePlaceholder: string = 'Select how was work done at the company';

  //Variables for additionalInformation
  additionalInformationLabel: string = 'Additional Information';
  additionalInformationErrorMessage: string = '';
  additionalInformationPlaceholder: string =
    'If you have any additional information to add, please enter it here';

  //Variables for the form error message
  formErrorMessage: string = '';
  formErrorMessageColor: string = 'red';

  //Variables for the warning box
  warningBoxTitleText: string = 'Warning';
  floatingWarningBoxMessage = 'Message';
  floatingWarningBoxMessageColor: string = 'blue';

  fetchEmploymentTypes(): void {
    console.log('Fetching employment types...');
    this.referenceService.getEmploymentTypes().subscribe((response) => {
      if (response.success === 'true') {
        console.log('Employment types fetched successfully');
        console.log(response.data);
        this.employmentTypes = response.data;
        console.log(this.employmentTypes);
      } else {
        console.log('Employment types fetched unsuccessfully');
        this.employmentTypes = [
          {
            id: 1,
            name: 'Full-Time',
            description: 'Employed on a full-time basis',
          },
        ];
        this.employmentTypeErrorMessage =
          'Only Full-Time employment type is currently available';
      }

      this.employmentTypeOptions = this.employmentTypes.map(
        (employmentTypeOption) => {
          return {
            name: employmentTypeOption.name,
            value: employmentTypeOption.id,
          };
        }
      );
    });
  }

  //Method for fetching industry classifications
  fetchIndustryClassifications(): void {
    this.referenceService.GetIndustryClassifications().subscribe((response) => {
      if (response.success === 'true') {
        console.log('Industry classifications fetched successfully');
        console.log(response.data);
        this.industryClassifications = response.data;
        console.log(this.industryClassifications);
      }
      if (response.success === 'false') {
        console.log('Industry classifications fetched unsuccessfully');
        this.industryClassifications = [
          {
            industryClassificationId: 'e5ba0f49-a930-4b32-be88-309274c57b7f',
            industry: 'Unspecified',
            sector: 'Unspecified',
          },
        ];
      }

      this.industryClassificationOptions = this.industryClassifications.map(
        (industryClassificationOption) => {
          return {
            name: `${industryClassificationOption.industry}(${industryClassificationOption.sector})`,
            value: industryClassificationOption.industryClassificationId,
          };
        }
      );
    });
  }

  fetchWorkTypes(): void {
    this.referenceService.GetWorkTypes().subscribe((response) => {
      if (response.success === 'true') {
        console.log('Work types fetched successfully');
        console.log(response.data);
        this.workTypes = response.data;
        console.log(this.workTypes);
      }
      if (response.success === 'false') {
        console.log('Work types fetched unsuccessfully');
      }
      this.workTypeOptions = this.workTypes.map((workTypeOption) => {
        return {
          name: workTypeOption.name,
          value: workTypeOption.workTypeId,
        };
      });
    });
  }

  getEmploymentTypeName(employmentTypeId: number): string {
    console.log('Finding employment type for ID:', employmentTypeId);
    const employmentType = this.employmentTypes.find(
      (item) => item.id === employmentTypeId
    );
    if (!employmentType) {
      console.error(
        `No matching employment type found for ID: ${employmentTypeId}`
      );
    }
    console.log('Employment type name:', employmentType?.name ?? '');
    return employmentType?.name ?? '';
  }

  getIndustryClassificationsIndustryAndSector(
    industryClassificationId: string
  ): string {
    console.log('Get industry by this id: ', industryClassificationId);
    const industryClassification = this.industryClassifications.find(
      (industryClassification) =>
        industryClassification.industryClassificationId ===
        industryClassificationId
    );
    console.log('Found industry: ', industryClassification);
    console.log(
      `${industryClassification?.industry}(${industryClassification?.sector})`
    );
    return `${industryClassification?.industry}(${industryClassification?.sector})`;
  }
  fetchAllData() {
    return forkJoin([
      this.referenceService.getEmploymentTypes(),
      this.referenceService.GetIndustryClassifications(),
      this.referenceService.GetWorkTypes(),
    ]).pipe(
      tap(
        ([
          employmentTypesResponse,
          industryClassificationsResponse,
          workTypesResponse,
        ]) => {
          //Handle employment types
          if (employmentTypesResponse.success === 'true') {
            this.employmentTypes = employmentTypesResponse.data.map(
              (item: any) => {
                return {
                  id: item.employmentTypeId,
                  name: item.name,
                  description: item.description,
                };
              }
            );
          }
          if (employmentTypesResponse.success === 'false') {
            this.employmentTypes = [
              {
                id: 1,
                name: 'Full-Time',
                description: 'Employed on a full-time basis',
              },
            ];
            this.employmentTypeErrorMessage =
              'Only Full-Time employment type is currently available';
          }
          this.employmentTypeOptions = this.employmentTypes.map(
            (employmentTypeOption) => {
              return {
                name: employmentTypeOption.name,
                value: employmentTypeOption.id,
              };
            }
          );

          //Handle industry classifications
          if (industryClassificationsResponse.success === 'true') {
            this.industryClassifications = industryClassificationsResponse.data;
          }
          if (industryClassificationsResponse.success === 'false') {
            this.industryClassifications = [
              {
                industryClassificationId:
                  'e5ba0f49-a930-4b32-be88-309274c57b7f',
                industry: 'Unspecified',
                sector: 'Unspecified',
              },
            ];
          }
          this.industryClassificationOptions = this.industryClassifications.map(
            (industryClassificationOption) => {
              return {
                name: `${industryClassificationOption.industry}(${industryClassificationOption.sector})`,
                value: industryClassificationOption.industryClassificationId,
              };
            }
          );

          //Handle work types
          if (workTypesResponse.success === 'true') {
            this.workTypes = workTypesResponse.data;
          }
          if (workTypesResponse.success === 'false') {
            this.workTypes = [
              {
                workTypeId: 2,
                name: 'Onsite',
                description: 'Work performed entirely on premises.',
              },
            ];
          }
          this.workTypeOptions = this.workTypes.map((workTypeOption) => {
            return {
              name: workTypeOption.name,
              value: workTypeOption.workTypeId,
            };
          });
        }
      )
    );
  }

  checkCompanyName(): void {
    const errors = this.careerInformationFormGroup.controls.companyName.errors;
    if (errors) {
      if (errors['required']) {
        this.companyNameErrorMessage = 'This field is required';
      }
      if (errors['pattern']) {
        this.companyNameErrorMessage =
          'Company name cannot start and/or end with a space';
      }
    }
    if (!errors) {
      this.companyNameErrorMessage = '';
    }
  }
  checkCompanyWebsite() {
    const errors =
      this.careerInformationFormGroup.controls.companyWebsite.errors;
    if (errors) {
      if (errors['pattern']) {
        this.companyWebsiteErrorMessage = 'Please enter a valid website URL';
      }
    }
    if (!errors) {
      this.companyWebsiteErrorMessage = '';
    }
  }

  checkJobTitle() {
    const errors = this.careerInformationFormGroup.controls.jobTitle.errors;
    if (errors) {
      if (errors['required']) {
        this.jobTitleErrorMessage = 'This field is required';
      }
      if (errors['pattern']) {
        this.jobTitleErrorMessage =
          'Job title cannot start and/or end with a space';
      }
    }
    if (!errors) {
      this.jobTitleErrorMessage = '';
    }
  }
  checkPosition() {
    const errors = this.careerInformationFormGroup.controls.position.errors;
    if (errors) {
      if (errors['pattern']) {
        this.positionErrorMessage =
          'Position cannot start and/or end with a space';
      }
    }
    if (!errors) {
      this.positionErrorMessage = '';
    }
  }
  checkCityOfEmployment() {
    const errors =
      this.careerInformationFormGroup.controls.cityOfEmployment.errors;
    if (errors) {
      if (errors['required']) {
        this.cityOfEmploymentErrorMessage = 'This field is required';
      }
      if (errors['pattern']) {
        this.cityOfEmploymentErrorMessage =
          'City of employment cannot start and/or end with a space';
      }
    }
    if (!errors) {
      this.cityOfEmploymentErrorMessage = '';
    }
  }
  checkCountryOfEmployment() {
    const errors =
      this.careerInformationFormGroup.controls.countryOfEmployment.errors;
    if (errors) {
      if (errors['required']) {
        this.countryOfEmploymentErrorMessage = 'This field is required';
      }
      if (errors['pattern']) {
        this.countryOfEmploymentErrorMessage =
          'Country of employment cannot start and/or end with a space';
      }
    }
    if (!errors) {
      this.countryOfEmploymentErrorMessage = '';
    }
  }
  checkEmploymentType() {
    const errors =
      this.careerInformationFormGroup.controls.employmentType.errors;
    if (errors) {
      if (errors['required']) {
        this.employmentTypeErrorMessage = 'This field is required';
      }
    }
    if (!errors) {
      this.employmentTypeErrorMessage = '';
    }
  }
  checkStartDate() {
    const errors = this.careerInformationFormGroup.controls.startDate.errors;
    if (errors) {
      if (errors['required']) {
        this.employmentTypeErrorMessage = 'This field is required';
      }
    }
    if (!errors) {
      this.employmentTypeErrorMessage = '';
    }
  }
  checkJobDescription() {
    const errors =
      this.careerInformationFormGroup.controls.jobDescription.errors;
    if (errors) {
      if (errors['pattern']) {
        this.jobDescriptionErrorMessage =
          'Job description cannot start and/or end with a space';
      }
    } else {
      this.jobDescriptionErrorMessage = '';
    }
  }
  checkResponsibilities() {
    const errors =
      this.careerInformationFormGroup.controls.responsibilities.errors;
    if (errors) {
      if (errors['pattern']) {
        this.responsibilitiesErrorMessage =
          'Responsibilities cannot start and/or end with a space';
      }
    } else {
      this.responsibilitiesErrorMessage = '';
    }
  }

  checkAchievements() {
    const errors = this.careerInformationFormGroup.controls.achievements.errors;
    if (errors) {
      if (errors['pattern']) {
        console.log('Is pattern:', errors['pattern'] === null);

        this.achievementsErrorMessage =
          'Achievements cannot start and/or end with a space';
      }
    } else {
      this.achievementsErrorMessage = '';
    }
  }

  checkIndustry() {
    const errors = this.careerInformationFormGroup.controls.industry.errors;
    if (errors) {
      this.industryErrorMessage = 'This field is required';
    }
    if (!this.careerInformationFormGroup.controls.industry.errors) {
      this.industryErrorMessage = '';
    }
  }
  checkSkillsUsed() {
    const errors = this.careerInformationFormGroup.controls.skillsUsed.errors;
    console.log('Errors:', errors);
    if (errors) {
      if (errors['required']) {
        this.skillsUsedErrorMessage = 'This field is required';
      }
      if (errors['pattern']) {
        this.skillsUsedErrorMessage =
          'Skills used cannot start and/or end with a space';
      }
    }
    if (!errors) {
      this.skillsUsedErrorMessage = '';
    }
  }

  checkAdditionalInformation() {
    const errors =
      this.careerInformationFormGroup.controls.additionalInformation.errors;
    if (errors) {
      if (errors['pattern']) {
        this.additionalInformationErrorMessage =
          'Additional information cannot start and/or end with a space';
      }
    }
    if (!errors) {
      this.additionalInformationErrorMessage = '';
    }
  }

  saveCareerInformationLog(): void {
    const saveRequest: CareerInformationHttpSaveRequest = {
      companyName: this.careerInformationFormGroup.value.companyName as string,
      companyWebsite: this.careerInformationFormGroup.value
        .companyWebsite as string,
      jobTitle: this.careerInformationFormGroup.value.jobTitle as string,
      position: this.careerInformationFormGroup.value.position as string,
      cityOfEmployment: this.careerInformationFormGroup.value
        .cityOfEmployment as string,
      countryOfEmployment: this.careerInformationFormGroup.value
        .countryOfEmployment as string,
      employmentTypeId: Number.parseInt(
        this.careerInformationFormGroup.value.employmentType as string
      ),
      startDate: this.careerInformationFormGroup.value.startDate as string,
      endDate: this.careerInformationFormGroup.value.endDate as string,
      jobDescription: this.careerInformationFormGroup.value
        .jobDescription as string,
      responsibilities: this.careerInformationFormGroup.value
        .responsibilities as string,
      achievements: this.careerInformationFormGroup.value
        .achievements as string,
      industryClassificationId: this.careerInformationFormGroup.value
        .industry as string,
      skillsUsed: this.careerInformationFormGroup.value.skillsUsed as string,
      workTypeId: this.careerInformationFormGroup.value.workTypeId
        ? Number.parseInt(
            this.careerInformationFormGroup.value.workTypeId as string
          )
        : null,
      additionalInformation: this.careerInformationFormGroup.value
        .additionalInformation as string,
    };
    this.accountService
      .createCareerInformation(saveRequest)
      .subscribe((response) => {
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;
        if (response.success === 'true') {
          this.floatingWarningBoxMessage = 'Data  saved successfully';
          this.floatingWarningBoxMessageColor = 'green';
        }
      });
  }

  updateCareerInformation() {
    console.log('Updating career information');
    console.log('Update values: ', this.changedValues);

    this.isDataTransmissionActive = true;
    this.isDataTransmissionComplete = false;
    this.floatingWarningBoxMessage = 'Saving changes...';

    const updatedCareerInformation: CareerInformationHttpUpdateRequest = {
      personCareerInformationId: this.careerInformation
        ?.personCareerInformationId as string,
      companyName: null,
      updateCompanyName: false,
      companyWebsite: null,
      updateCompanyWebsite: false,
      jobTitle: null,
      updateJobTitle: false,
      position: null,
      updatePosition: false,
      cityOfEmployment: null,
      updateCityOfEmployment: false,
      countryOfEmployment: null,
      updateCountryOfEmployment: false,
      employmentTypeId: null,
      updateEmploymentTypeId: false,
      startDate: null,
      updateStartDate: false,
      endDate: null,
      updateEndDate: false,
      jobDescription: null,
      updateJobDescription: false,
      responsibilities: null,
      updateResponsibilities: false,
      achievements: null,
      updateAchievements: false,
      industryClassificationId: null,
      updateIndustryClassificationId: false,
      skillsUsed: null,
      updateSkillsUsed: false,
      workTypeId: null,
      updateWorkTypeId: false,
      additionalInformation: null,
      updateAdditionalInformation: false,
    };

    Object.keys(this.changedValues).forEach((field) => {
      console.log('Field: ', field);
      console.log('Field value: ', this.changedValues[field]);
      console.log('Field type:', typeof this.changedValues[field]);
      const typedKey = field as keyof CareerInformationHttpUpdateRequest;
      console.log('Typed key: ', typedKey);
      const typedKeyFieldUpdate = `update${typedKey
        .at(0)
        ?.toUpperCase()}${typedKey.slice(1)}`;

      console.log('Typed key field update: ', typedKeyFieldUpdate);

      const typedUpdateFieldKey =
        typedKeyFieldUpdate as keyof CareerInformationHttpUpdateRequest;
      console.log('Typed update field key: ', typedUpdateFieldKey);
      console.log('Typed update field key type: ', typeof typedUpdateFieldKey);

      console.log(
        'Updated career information field: ',
        updatedCareerInformation[typedKey]
      );
      console.log(
        'Update career information field flag: ',
        updatedCareerInformation[typedUpdateFieldKey]
      );
      const updatedValue = this.changedValues[field];
      console.log('Updated value: ', updatedValue);
      console.log('Updated value type: ', typeof updatedValue);

      // Check if the key exists in updatedCareerInformation before assigning
      if (typedKey in updatedCareerInformation) {
        (updatedCareerInformation[typedKey] as any) = updatedValue;
        if (typedUpdateFieldKey in updatedCareerInformation) {
          (updatedCareerInformation[typedUpdateFieldKey] as any) = true;
        }
      }

      console.log(
        'Updated career information field: ',
        updatedCareerInformation[typedKey]
      );
    });

    console.log('Updated career information: ', updatedCareerInformation);
    this.accountService
      .updateCareerInformation(updatedCareerInformation)
      .subscribe((response) => {
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;
        if (response.success === 'true') {
          this.continueButtonText = 'See all careers';
          this.floatingWarningBoxMessage = 'Changes have been saved';
          this.floatingWarningBoxMessageColor = 'green';

          this.goBackButtonText = 'Back to career information page';
          this.goBackButtonColor = 'orange';
        }
        if (response.success === 'false') {
          this.continueButtonText = 'See all careers';
          this.floatingWarningBoxMessage = response.message;
          this.floatingWarningBoxMessageColor = 'red';

          this.goBackButtonText = 'Back to career information page';
          this.goBackButtonColor = 'orange';
        }
      });
  }

  //Method to delete the selected career information
  deleteCareerInformation() {
    console.log('Deleting career information');
    this.isDataTransmissionActive = true;
    this.isDataTransmissionComplete = false;
    this.floatingWarningBoxMessage = 'Deleting career information...';
    this.accountService
      .deleteCareerInformation(
        this.careerInformation?.personCareerInformationId as string
      )
      .subscribe((response) => {
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;
        if (response.success === 'true') {
          this.floatingWarningBoxMessage = 'Career item deleted successfully';
          this.floatingWarningBoxMessageColor = 'green';
        }
        if (response.success === 'false') {
          this.floatingWarningBoxMessage = response.message;
          this.floatingWarningBoxMessageColor = 'red';
        }
      });
  }
  optionalParameterRegExValidator(pattern: RegExp) {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) {
        return null;
      }
      return pattern.test(value) ? null : { pattern: true };
    };
  }

  toggleWarningBox() {
    this.isWarningDialogOpen = !this.isWarningDialogOpen;
  }
  closeEditModal(): void {
    this.closeCareerInformationLog.emit();
  }

  //Method to check if the values have been changed in the form
  //The function has two operations types, depending on the component mode
  //Component create mode: Check if the values are different from the default values
  //Component edit mode: Check if the values are different from the values that were saved in the database
  checkForChanges() {
    //Check if the companyName was changed
    if (
      this.careerInformationFormGroup.controls.companyName.value !==
      this.initialValues['companyName']
    ) {
      this.hasChanges = true;
      return;
    }

    //Check if the companyWebsite was changed
    if (
      this.careerInformationFormGroup.controls.companyWebsite.value !==
      this.initialValues['companyWebsite']
    ) {
      this.hasChanges = true;
      return;
    }
    //Check if the jobTitle was changed
    if (
      this.careerInformationFormGroup.controls.jobTitle.value !==
      this.initialValues['jobTitle']
    ) {
      this.hasChanges = true;
      return;
    }
    //Check if the position was changed
    if (
      this.careerInformationFormGroup.controls.position.value !==
      this.initialValues['position']
    ) {
      this.hasChanges = true;
      return;
    }
    //Check if the cityOfEmployment was changed
    if (
      this.careerInformationFormGroup.controls.cityOfEmployment.value !==
      this.initialValues['cityOfEmployment']
    ) {
      this.hasChanges = true;
      return;
    }
    //Check if the countryOfEmployment was changed
    if (
      this.careerInformationFormGroup.controls.countryOfEmployment.value !==
      this.initialValues['countryOfEmployment']
    ) {
      this.hasChanges = true;
      return;
    }
    //Check if the employmentType was changed
    if (
      this.careerInformationFormGroup.controls.employmentType.value !==
      this.initialValues['employmentType']
    ) {
      this.hasChanges = true;
      return;
    }
    //Check if the startDate was changed
    if (
      this.careerInformationFormGroup.controls.startDate.value !==
      this.initialValues['startDate']
    ) {
      this.hasChanges = true;
      return;
    }
    //Check if the endDate was changed
    //Because the endDate initial value is null, if the endDate value is typeof string and it's values is equal to '', then it means the value was not changed, and the endDate value is set as null
    if (
      this.careerInformationFormGroup.controls.endDate.value !== null &&
      typeof this.careerInformationFormGroup.controls.endDate.value ===
        'string' &&
      this.careerInformationFormGroup.controls.endDate.value === ''
    ) {
      this.careerInformationFormGroup.controls.endDate.setValue(null);
    }
    if (
      this.careerInformationFormGroup.controls.endDate.value !==
      this.initialValues['endDate']
    ) {
      this.hasChanges = true;
      return;
    }

    //Check if the jobDescription was changed
    if (
      this.careerInformationFormGroup.controls.jobDescription.value !==
      this.initialValues['jobDescription']
    ) {
      this.hasChanges = true;
      return;
    }

    //Check if the responsibilities was changed
    if (
      this.careerInformationFormGroup.controls.responsibilities.value !==
      this.initialValues['responsibilities']
    ) {
      this.hasChanges = true;
      return;
    }

    //Check if the achievements was changed
    if (
      this.careerInformationFormGroup.controls.achievements.value !==
      this.initialValues['achievements']
    ) {
      this.hasChanges = true;
      return;
    }

    //Check if the industry was changed
    if (
      this.careerInformationFormGroup.controls.industry.value !==
      this.initialValues['industry']
    ) {
      this.hasChanges = true;
      return;
    }

    //Check if the skillsUsed was changed
    if (
      this.careerInformationFormGroup.controls.skillsUsed.value !==
      this.initialValues['skillsUsed']
    ) {
      this.hasChanges = true;
      return;
    }

    //Check if the workType was changed
    if (
      this.careerInformationFormGroup.controls.workTypeId.value !==
      this.initialValues['workTypeId']
    ) {
      console.log('workTypeId changed');
      console.log('Old workTypeId:', this.initialValues['workTypeId']);
      console.log(
        'New workTypeId:',
        this.careerInformationFormGroup.controls.workTypeId.value
      );
      this.hasChanges = true;
      return;
    }

    //Check if the additionalInformation was changed
    if (
      this.careerInformationFormGroup.controls.additionalInformation.value !==
      this.initialValues['additionalInformation']
    ) {
      this.hasChanges = true;
      return;
    }
    this.hasChanges = false;
    console.log('Has changes:', this.hasChanges);
  }

  //Function to set the save button values, depending on the mode of the component
  //Create mode - Save data
  //Edit mode - Save changes
  setSaveButtonValues() {
    this.saveButtonColor = 'green';
    if (this.isCreateOrEditMode) {
      this.saveButtonText = 'Save data';
    }
    if (!this.isCreateOrEditMode) {
      this.saveButtonText = 'Save changes';
    }
  }

  //Function to set the discard button values, depending on the mode of the component
  //Create mode - Discard data
  //Edit mode - Discard changes
  setDiscardButtonValues() {
    this.discardButtonColor = 'red';
    if (this.isCreateOrEditMode) {
      this.discardButtonText = 'Discard data';
    }
    if (!this.isCreateOrEditMode) {
      this.discardButtonText = 'Discard changes';
    }
  }

  //Function to go back once the data transmission is complete
  goBackToCareerInformation() {
    this.toggleWarningBox();
    this.closeEditModal();
  }

  //Function to go to all career information
  goToAllCareerInformation() {
    this.toggleWarningBox();
    this.closeEditModal();
  }

  //Function to call all the validation functions
  checkForErrorsInTheForm() {
    this.checkCompanyName();
    this.checkCompanyWebsite();
    this.checkJobTitle();
    this.checkPosition();
    this.checkCityOfEmployment();
    this.checkCountryOfEmployment();
    this.checkEmploymentType();
    this.checkStartDate();
    this.checkJobDescription();
    this.checkResponsibilities();
    this.checkAchievements();
    this.checkIndustry();
    this.checkSkillsUsed();
    this.checkAdditionalInformation();
  }
  saveButtonClick() {
    this.isSaveButtonClicked = true;
    //Check if the form is valid
    if (this.careerInformationFormGroup.invalid) {
      this.checkForErrorsInTheForm();
      this.formErrorMessage = 'Please resolve any errors before saving.';
      this.formErrorMessageColor = 'red';

      return;
    }
    console.log('Save data active');
    //If the form is valid, then check in which mode the component is
    if (this.isCreateOrEditMode) {
      //The component is in create mode - data is to be saved
      console.log('Saving data...');
      //Set the current data transmission operation to create
      this.isCreateOrUpdateOperation = true;
      this.floatingWarningBoxMessage =
        'Are you sure you want to save the data?';
      this.floatingWarningBoxMessageColor = 'orange';

      //Set the confirm button text value
      this.confirmButtonText = 'Save data';

      this.toggleWarningBox();
    }
    if (!this.isCreateOrEditMode) {
      //The component is in edit mode - data is to be updated
      console.log('Saving changes...');
      //Set the current data transmission operation to create
      this.isCreateOrUpdateOperation = false;
      this.floatingWarningBoxMessage =
        'Are you sure you want to save the changes?';
      this.floatingWarningBoxMessageColor = 'orange';
      this.confirmButtonText = 'Save changes';
      this.confirmButtonColor = 'green';
      this.toggleWarningBox();
    }
  }

  //Function used for the delete button
  deleteButtonClick() {
    this.isDeleteOperation = true;
    this.floatingWarningBoxMessage =
      'Are you sure you want to delete this item, its content will be lost permanently?';
    this.warningBoxTitleText = 'Delete item';
    this.floatingWarningBoxMessageColor = 'red';
    this.confirmButtonColor = 'red';
    this.confirmButtonText = 'Yes, delete item';
    this.cancelButtonColor = 'green';
    this.cancelButtonText = "No, don't delete the item";
    this.toggleWarningBox();
  }

  discardButtonClick() {
    this.isDiscardButtonClicked = true;
    //Check the component mode
    if (this.isCreateOrEditMode) {
      //The component is in create mode - data is to be discarded
      console.log('Discarding data...');
      this.floatingWarningBoxMessage =
        'Are you sure you want to discard the data, it will not be saved?';
      this.floatingWarningBoxMessageColor = 'red';
      this.confirmButtonText = 'Yes, discard data';
      this.confirmButtonColor = 'red';
      this.cancelButtonColor = 'green';
      this.cancelButtonText = "No, don't discard the data";
      this.toggleWarningBox();
    }
    if (!this.isCreateOrEditMode) {
      //The component is in create mode - data is to be discarded
      console.log('Discarding data...');
      this.floatingWarningBoxMessage =
        "Are you sure you want to discard the changes, they won't be applied?";
      this.floatingWarningBoxMessageColor = 'red';
      this.confirmButtonText = 'Yes, discard data';
      this.confirmButtonColor = 'red';
      this.cancelButtonColor = 'green';
      this.cancelButtonText = "No, don't discard the data";
      this.toggleWarningBox();
    }
  }
  //Function to be used for the confirm button in the floating warning box
  confirmButtonClick() {
    //Set the flag to signal that the data transmission to the server is active
    this.isDataTransmissionActive = true;

    console.log('Confirm button clicked');

    //Check if the discard button was clicked
    if (this.isDiscardButtonClicked) {
      console.log('Discarding data...');
      this.floatingWarningBoxMessage = 'Discarding data...';
      this.floatingWarningBoxMessageColor = 'red';
      this.goBackToCareerInformation();
    }
    //Check if the selected operation is a delete operation
    if (this.isDeleteOperation) {
      console.log('Deleting data...');
      this.floatingWarningBoxMessage = 'Deleting item...';
      this.floatingWarningBoxMessageColor = 'red';
      this.deleteCareerInformation();
    }
    if (!this.isDeleteOperation && !this.isDiscardButtonClicked) {
      //Check the mode of the component
      if (this.isCreateOrEditMode) {
        console.log('Saving data...');
        this.floatingWarningBoxMessage = 'Saving data...';

        this.saveCareerInformationLog();
      }
      if (!this.isCreateOrEditMode && !this.isDiscardButtonClicked) {
        console.log('Saving changes...');
        this.floatingWarningBoxMessage = 'Saving changes...';
        this.updateCareerInformation();
      }
    }
  }

  //Function to be used for the cancel button in the floating warning box, closes the floating warning box
  cancelButtonClick() {
    if (this.isDiscardButtonClicked) {
      this.isDiscardButtonClicked = false;
    }
    this.toggleWarningBox();
  }

  //Function to be used for the continue button in the floating warning box, closes the floating warning box and the edit modal
  continueButtonClick() {
    this.toggleWarningBox();
    this.closeEditModal();
  }

  //Function to be used for the go back button in the floating warning box, closes the floating warning box
  goBackToEditModal() {
    this.toggleWarningBox();
  }
}
