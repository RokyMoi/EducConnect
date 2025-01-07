import { Component, inject, Input, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  FormsModule,
  Validators,
} from '@angular/forms';
import { TextInputComponentComponent } from '../../../common/input/text/text-input-component/text-input-component.component';
import { TextAreaInputComponentComponent } from '../../../common/input/text/text-area-input-component/text-area-input-component.component';
import { ReferenceService } from '../../../services/reference/reference.service';
import { LearningCategory } from '../../../_models/reference/learning-category/learning-category.reference.model';
import { SelectDropdownComponent } from '../../../common/select/select-dropdown/select-dropdown.component';
import { SelectDropdownGroupsComponent } from '../../../common/select/select-dropdown-groups/select-dropdown-groups.component';
import { LearningSubcategory } from '../../../_models/reference/learning-subcategory';
import { LearningDifficultyLevel } from '../../../_models/reference/learning-difficulty-level';
import { CourseType } from '../../../_models/reference/course-type';
import { NgFor, NgIf } from '@angular/common';
import { RadioButtonGroupComponent } from '../../../_models/reference/radio-button-group/radio-button-group.component';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import { CourseCreateService } from '../../../services/course/course-create-service.service';
import { CreateCourseBasicInformation } from '../../../_models/course/create-course/create-course.create-course.course.model';
import { FloatingWarningBoxComponent } from '../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';

@Component({
  standalone: true,
  selector: 'app-course-basic-information',
  imports: [
    ReactiveFormsModule,
    TextInputComponentComponent,
    TextAreaInputComponentComponent,
    SelectDropdownComponent,
    SelectDropdownGroupsComponent,
    FormsModule,
    RadioButtonGroupComponent,
    SubmitButtonComponent,
    FloatingWarningBoxComponent,
    MatProgressSpinner,
    NgIf,
  ],
  templateUrl: './course-basic-information.component.html',
  styleUrl: './course-basic-information.component.css',
})
export class CourseBasicInformationComponent implements OnInit {
  @Input() componentTitle: string = 'Enter basic information this course';
  @Input() referenceService!: ReferenceService;

  //Injected services
  courseCreateService = inject(CourseCreateService);
  //Variables for the course name
  courseNameLabel: string = 'Course Name';
  courseNamePlaceholder: string = 'Enter the course name';
  courseNameErrorMessage: string = '';

  //Variables for the course subject
  courseSubjectLabel: string = 'Course Subject';
  courseSubjectPlaceholder: string =
    'Tell the students what is this course about';
  courseSubjectErrorMessage: string = '';

  //Variables for the course description
  courseDescriptionLabel: string = 'Course Description';
  courseDescriptionPlaceholder: string =
    'Describe in detail what this course is about';
  courseDescriptionErrorMessage: string = '';

  //Variables for the learning subcategory
  courseLearningSubcategoryLabel: string = 'Course subcategory';
  courseLearningSubcategoryPlaceholder: string =
    'Select a subcategory to which this course belongs';
  courseLearningSubcategoryErrorMessage: string = '';
  //Arrays for holding values for the course learning categories and subcategories
  learningCategories: LearningCategory[] = [];
  learningCategoryOptions: { name: string; value: string }[] = [];

  learningSubcategories: LearningSubcategory[] = [];
  learningSubcategoryOptions: { name: string; value: string }[] = [];

  learningCategoriesAndSubcategoriesMap: {
    label: string;
    items: { name: string; value: string }[];
  }[] = [];

  //Variables for the course learning difficulty level
  courseLearningDifficultyLevelLabel: string = 'Course difficulty level';
  courseLearningDifficultyLevelPlaceholder: string =
    'Select a level of difficulty for this course';
  courseLearningDifficultyLevelErrorMessage: string = '';

  //Arrays for holding values for the course learning difficulty levels
  learningDifficultyLevels: LearningDifficultyLevel[] = [];
  learningDifficultyLevelOptions: { name: string; value: string }[] = [];

  //Variables for the course types
  courseTypeWarningMessage: string = '';
  courseTypeWarningMessageColor: string = 'black';

  //Arrays for holding values for the course types
  courseTypes: CourseType[] = [];
  courseTypeOptions: { name: string; value: string }[] = [];

  //Variables for the course price
  coursePriceLabel: string = 'Course price';
  coursePricePlaceholder: string =
    'Enter how much you want to charge for this course';
  coursePriceErrorMessage: string = '';

  //Variables for the submit button
  submitButtonText: string = 'Next step';
  submitButtonColor: string = '';

  //Variables for the form error message
  formErrorMessage: string = '';
  formErrorMessageColor: string = 'red';

  //Variables for the operation of data transmission to the backend

  //Variables for the floating warning box that appears when the form is submitted
  showFloatingWarningBox: boolean = false;
  floatingWarningBoxTitle: string = 'Warning';
  floatingWarningBoxMessage: string = '';
  floatingWarningBoxMessageColor: string = 'green';

  //Text to display once the data has been retrieved from the server
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';
  isDataTransmissionActive: boolean = false;
  isDataTransmissionComplete: boolean = false;
  isOperationSuccessful: boolean = false;

  //Variables for the button that is shown within the floating warning box, but only if the course is saved successfully
  continueToNextStepButtonText: string = 'Continue to the next step';
  continueToNextStepButtonColor: string = 'green';

  //Variables for the button that is shown within the floating warning box, but only if the operation of saving the course failed

  goBackButtonText: string = 'Edit the data';
  goBackButtonColor: string = 'orange';

  //Common variables for all the buttons in the floating warning box
  buttonMargin: string = '12px';

  //Flag to specify if the step is completed
  isStepCompleted: boolean = false;
  courseBasicInformationFormGroup = new FormGroup({
    courseName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(100),
    ]),
    courseSubject: new FormControl(
      '',

      [Validators.required, Validators.minLength(10), Validators.maxLength(200)]
    ),
    courseDescription: new FormControl(
      '',

      [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(1024),
      ]
    ),
    learningSubcategory: new FormControl('', [Validators.required]),
    learningDifficultyLevel: new FormControl('', [Validators.required]),
    courseType: new FormControl('', [Validators.required]),
    coursePrice: new FormControl('', [
      Validators.required,
      Validators.min(0),
      Validators.max(99999),
      Validators.pattern(/^\d+(\.\d{1,2})?$/),
    ]),
  });

  ngOnInit(): void {
    this.getLearningCategoriesAndSubcategories();
    this.getLearningDifficultyLevels();
    this.getCourseTypes();

    this.courseBasicInformationFormGroup.controls.courseName.valueChanges.subscribe(
      () => {
        this.checkForCourseNameErrors();
      }
    );

    this.courseBasicInformationFormGroup.controls.courseSubject.valueChanges.subscribe(
      () => {
        this.checkForCourseSubjectErrors();
      }
    );

    this.courseBasicInformationFormGroup.controls.courseDescription.valueChanges.subscribe(
      () => {
        this.checkForCourseDescriptionErrors();
      }
    );

    this.courseBasicInformationFormGroup.controls.learningSubcategory.valueChanges.subscribe(
      () => {
        this.checkForCourseLearningSubcategoryErrors();
      }
    );

    this.courseBasicInformationFormGroup.controls.learningDifficultyLevel.valueChanges.subscribe(
      () => {
        this.checkForLearningDifficultyLevelErrors();
      }
    );

    this.courseBasicInformationFormGroup.controls.courseType.valueChanges.subscribe(
      () => {
        console.log('Course type changed');
        this.setCourseTypeDescription(
          this.courseBasicInformationFormGroup.controls.courseType
            .value as string
        );
      }
    );

    this.courseBasicInformationFormGroup.controls.coursePrice.valueChanges.subscribe(
      () => {
        this.checkForCoursePriceErrors();
      }
    );
  }

  getLearningCategoriesAndSubcategories() {
    this.referenceService
      .getLearningCategoriesAndSubcategories()
      .subscribe((response) => {
        console.log(response);
        if (response.success === 'true') {
          this.learningCategories = response.data.learningCategories;
          this.learningCategoryOptions = this.learningCategories.map(
            (learningCategory) => {
              return {
                name: learningCategory.learningCategoryName,
                value: learningCategory.learningCategoryId,
              };
            }
          );
          this.learningSubcategories = response.data.learningSubcategories;
          console.log(this.learningSubcategories);
          this.learningSubcategoryOptions = this.learningSubcategories.map(
            (learningSubcategory) => {
              return {
                name: learningSubcategory.learningSubcategoryName,
                value: learningSubcategory.learningSubcategoryId,
              };
            }
          );
          console.log(this.learningSubcategoryOptions);
          this.learningCategoriesAndSubcategoriesMap =
            this.learningCategories.map((category) => {
              const subcategories = this.learningSubcategories.filter(
                (subcategory) =>
                  subcategory.learningCategoryId === category.learningCategoryId
              );
              return {
                label: category.learningCategoryName,
                items: subcategories.map((subcategory) => {
                  return {
                    name: subcategory.learningSubcategoryName,
                    value: subcategory.learningSubcategoryId,
                  };
                }),
              };
            });
          console.log(this.learningCategoriesAndSubcategoriesMap);
        }
        if (response.success === 'false') {
          console.log(response.message);
        }
      });
  }

  getLearningDifficultyLevels() {
    this.referenceService
      .getAllLearningDifficultyLevels()
      .subscribe((response) => {
        console.log(response);
        if (response.success === 'true') {
          this.learningDifficultyLevels = response.data.learningDifficultyLevel;
          console.log(this.learningDifficultyLevels);
          this.learningDifficultyLevelOptions =
            this.learningDifficultyLevels.map((level) => {
              return {
                name: level.name,
                value: level.learningDifficultyLevelId,
              };
            });
        }
        if (response.success === 'false') {
          console.log(response.message);
        }
      });
  }

  getCourseTypes() {
    this.referenceService.getAllCourseTypes().subscribe((response) => {
      console.log(response);
      if (response.success === 'true') {
        this.courseTypes = response.data;
        this.courseTypeOptions = this.courseTypes.map((type) => {
          return {
            name: type.name,
            value: type.courseTypeId.toString(),
          };
        });
        console.log(this.courseTypeOptions);
      }
      if (response.success === 'false') {
        console.error(response.message);
      }
    });
  }

  onCourseTypeSelected(newValue: string) {
    this.courseBasicInformationFormGroup.controls.courseType.setValue(newValue);
    console.log(this.courseBasicInformationFormGroup.value);
  }

  onCourseSubcategorySelect(event: Event) {
    console.log(event);
  }

  onSaveData() {
    console.log('Saving data...');
    const requestBody: CreateCourseBasicInformation = {
      courseName: this.courseBasicInformationFormGroup.controls.courseName
        .value as string,
      courseSubject: this.courseBasicInformationFormGroup.controls.courseSubject
        .value as string,
      courseDescription: this.courseBasicInformationFormGroup.controls
        .courseDescription.value as string,
      learningSubcategoryId: this.courseBasicInformationFormGroup.controls
        .learningSubcategory.value as string,
      learningDifficultyLevelId: Number.parseInt(
        this.courseBasicInformationFormGroup.controls.learningDifficultyLevel
          .value as string
      ),
      courseTypeId: Number.parseInt(
        this.courseBasicInformationFormGroup.controls.courseType.value as string
      ),
      coursePrice: Number.parseFloat(
        this.courseBasicInformationFormGroup.controls.coursePrice
          .value as string
      ),
    };
    this.courseCreateService
      .createCourseBasicInformation(requestBody)
      .subscribe((response) => {
        console.log(response);
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;
        if (response.success === 'true') {
          this.isOperationSuccessful = true;
          this.floatingWarningBoxMessage = 'Course created successfully';
          this.floatingWarningBoxMessageColor = 'green';
        }
        if (response.success === 'false') {
          this.isOperationSuccessful = false;
          this.floatingWarningBoxMessage =
            'Failed to save course, ' + response.message;
          this.floatingWarningBoxMessageColor = 'red';
        }
      });
  }

  checkForCourseNameErrors() {
    const errors =
      this.courseBasicInformationFormGroup.controls.courseName.errors;
    console.log(errors);
    if (errors) {
      console.log('errors');
      if (errors['required']) {
        console.log('errors required');
        this.courseNameErrorMessage = 'This field is required';
      }
      if (errors['minlength']) {
        this.courseNameErrorMessage =
          'Course name must be at least 3 characters long';
      }
      if (errors['maxlength']) {
        this.courseNameErrorMessage =
          'Course name must be at most 100 characters long';
      }
    }
    if (!errors) {
      this.courseNameErrorMessage = '';
    }
  }

  checkForCourseSubjectErrors() {
    const errors =
      this.courseBasicInformationFormGroup.controls.courseSubject.errors;
    console.log(errors);
    if (errors) {
      console.log('errors');
      if (errors['required']) {
        console.log('errors required');
        this.courseSubjectErrorMessage = 'This field is required';
      }
      if (errors['minlength']) {
        this.courseSubjectErrorMessage =
          'Course name must be at least 10 characters long';
      }
      if (errors['maxlength']) {
        this.courseSubjectErrorMessage =
          'Course name must be at most 200 characters long';
      }
    }
    if (!errors) {
      this.courseSubjectErrorMessage = '';
    }
  }

  checkForCourseDescriptionErrors() {
    const errors =
      this.courseBasicInformationFormGroup.controls.courseDescription.errors;
    console.log(errors);
    if (errors) {
      console.log('errors');
      if (errors['required']) {
        console.log('errors required');
        this.courseDescriptionErrorMessage = 'This field is required';
      }
      if (errors['minlength']) {
        this.courseDescriptionErrorMessage =
          'Course name must be at least 10 characters long';
      }
      if (errors['maxlength']) {
        this.courseDescriptionErrorMessage =
          'Course name must be at most 1024 characters long';
      }
    }
    if (!errors) {
      this.courseDescriptionErrorMessage = '';
    }

    console.log(this.courseBasicInformationFormGroup.value);
  }

  checkForCourseLearningSubcategoryErrors() {
    const errors =
      this.courseBasicInformationFormGroup.controls.learningSubcategory.errors;
    console.log(errors);

    if (errors) {
      this.courseLearningSubcategoryErrorMessage = 'This field is required';
    }
    if (!errors) {
      this.courseLearningSubcategoryErrorMessage = '';
    }
  }

  checkForLearningDifficultyLevelErrors() {
    const errors =
      this.courseBasicInformationFormGroup.controls.learningDifficultyLevel
        .errors;
    if (errors) {
      this.courseLearningDifficultyLevelErrorMessage = 'This field is required';
    }
    if (!errors) {
      this.courseLearningDifficultyLevelErrorMessage = '';
    }
  }

  setCourseTypeDescription(courseType: string) {
    this.courseTypeWarningMessage = this.courseTypes.find(
      (type) => type.courseTypeId === Number.parseInt(courseType)
    )?.description as string;
    this.courseTypeWarningMessageColor = 'black';
  }

  checkForCoursePriceErrors() {
    const errors =
      this.courseBasicInformationFormGroup.controls.coursePrice.errors;
    console.log(errors);
    if (errors) {
      console.log('errors');
      if (errors['required']) {
        console.log('errors required');
        this.coursePriceErrorMessage = 'This field is required';
      }
      if (errors['min']) {
        this.coursePriceErrorMessage =
          'Set price at 0 for free course, and above 0 for paid';
      }
      if (errors['max']) {
        this.coursePriceErrorMessage =
          'Price of the course cannot exceed 99999';
      }
      if (errors['pattern']) {
        this.coursePriceErrorMessage =
          'Course price can only have numbers, and a single decimal point, with 2 digits after the decimal point';
      }
    }
    if (!errors) {
      this.coursePriceErrorMessage = '';
    }
  }

  checkForErrors() {
    var hasErrors = false;

    if (this.courseBasicInformationFormGroup.controls.courseName.errors) {
      hasErrors = true;
      this.checkForCourseNameErrors();
    }

    if (this.courseBasicInformationFormGroup.controls.courseSubject.errors) {
      hasErrors = true;
      this.checkForCourseSubjectErrors();
    }
    if (
      this.courseBasicInformationFormGroup.controls.courseDescription.errors
    ) {
      hasErrors = true;
      this.checkForCourseDescriptionErrors();
    }

    if (
      this.courseBasicInformationFormGroup.controls.learningSubcategory.errors
    ) {
      hasErrors = true;
      this.checkForCourseLearningSubcategoryErrors();
    }

    if (
      this.courseBasicInformationFormGroup.controls.learningDifficultyLevel
        .errors
    ) {
      hasErrors = true;
      this.checkForLearningDifficultyLevelErrors();
    }

    if (
      (this.courseBasicInformationFormGroup.controls.courseType.value as string)
        .length === 0 ||
      (
        this.courseBasicInformationFormGroup.controls.courseType.value as string
      ).trim() === ''
    ) {
      hasErrors = true;
      this.courseTypeWarningMessage = 'This field is required';
      this.courseTypeWarningMessageColor = 'red';
    }

    if (this.courseBasicInformationFormGroup.controls.coursePrice.errors) {
      hasErrors = true;
      this.checkForCoursePriceErrors();
    }
    if (hasErrors) {
      console.log('Errors found');
      this.formErrorMessage = 'Please resolve any errors before proceeding';
      this.formErrorMessageColor = 'red';
    } else {
      console.log('No errors found');
      this.formErrorMessage = 'You can save the data, and proceed';
      this.formErrorMessageColor = 'red';
    }
  }

  onNextStep() {
    this.checkForErrors();
    this.courseBasicInformationFormGroup.valueChanges.subscribe(() => {
      this.checkForErrors();
    });

    if (this.courseBasicInformationFormGroup.valid) {
      this.showFloatingWarningBox = true;
      this.floatingWarningBoxTitle = 'Basic Information about course';
      this.floatingWarningBoxMessage = 'Saving data...';
      this.floatingWarningBoxMessageColor = 'orange';
      this.isDataTransmissionActive = true;
      this.onSaveData();
    }
  }

  //If the operation of saving the course fails, then reset the data transmission flags
  onGoBack() {
    this.isDataTransmissionActive = false;
    this.isDataTransmissionComplete = false;
    this.isOperationSuccessful = false;
    this.showFloatingWarningBox = false;
  }

  //If the operation of saving the course is successful, then reset the data transmission flags
  onContinue() {
    this.isDataTransmissionActive = false;
    this.isDataTransmissionComplete = false;
    this.isOperationSuccessful = false;
    this.isStepCompleted = true;

    //Open the next step
    console.log('Open next step');
  }
}
