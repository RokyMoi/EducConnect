import {
  Component,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  NgModel,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgxDocViewerModule } from 'ngx-doc-viewer';

import { TextInputComponentComponent } from '../../../../../common/input/text/text-input-component/text-input-component.component';
import { Form } from '@angular/forms';
import { TextAreaInputComponentComponent } from '../../../../../common/input/text/text-area-input-component/text-area-input-component.component';
import { TextAreaResizeType } from '../../../../../../enums/textarea-resize-types.enum';
import { Editor, NgxEditorModule } from 'ngx-editor';
import { CourseLessonSupplementaryMaterial } from '../../../../../models/course/course-lesson-supplementary-material/course-lesson-supplementary-material.model';
import { DomSanitizer } from '@angular/platform-browser';
import { NgFor, NgIf } from '@angular/common';
import { SubmitButtonComponent } from '../../../../../common/button/submit-button/submit-button.component';
import { distinctUntilChanged } from 'rxjs';
import { CourseCreateService } from '../../../../../services/course/course-create-service.service';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FloatingWarningBoxComponent } from '../../../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import { CourseLesson } from '../../../../../models/course/course-lesson/course-lesson.model';
import { CourseLessonContent } from '../../../../../models/course/course-lesson-content.model';
import { CourseLessonDialogComponent } from '../course-lesson-dialog/course-lesson-dialog.component';
import { FrontendToBackendOperationType } from '../../../../../../enums/frontend-to-backend-operation-type.enum';
import { CourseLessonUpdateRequest } from '../../../../../models/course/course-lesson/course-lesson-update-request.model';
import { CourseDeleteDialogComponent } from '../course-delete-dialog/course-delete-dialog.component';
import { CourseLessonShorthand } from '../../../../../models/course/course-lesson/course-lesson-shorthand.model';

@Component({
  standalone: true,
  selector: 'app-course-lesson-modal',
  imports: [
    ReactiveFormsModule,
    TextInputComponentComponent,
    TextAreaInputComponentComponent,
    NgxEditorModule,
    NgIf,
    SubmitButtonComponent,
    MatProgressSpinner,
    NgFor,
    MatProgressBarModule,
    FloatingWarningBoxComponent,
    CourseLessonDialogComponent,
    CourseDeleteDialogComponent,
    NgxDocViewerModule,
  ],
  templateUrl: './course-lesson-modal.component.html',
  styleUrl: './course-lesson-modal.component.css',
})
export class CourseLessonModalComponent implements OnInit {
  @Input() modalTitle: string = 'Modal Title';
  @Input() isCreateOrEditMode: boolean = true;
  @Input() courseId: string = 'e6c87eb6-f462-4516-8d17-ddbd9b3b1def';
  @Input() courseLessonId: string = '';
  @Input() courseLessonPosition: number = -1;
  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();
  @Output() closeDeleteDialogAndModalAndRefreshCourseList: EventEmitter<void> =
    new EventEmitter<void>();

  courseCreateService: CourseCreateService = inject(CourseCreateService);

  showCloseButton: boolean = true;

  sanitizer: DomSanitizer = inject(DomSanitizer);

  //Add Editor configuration
  public editor: Editor = new Editor();

  showSupplementaryMaterialUpload: boolean = false;

  showDialog: boolean = false;

  showDeleteDialog: boolean = false;

  showFilePreview: boolean = false;
  updatedCourseLessonValues: any = {
    courseLessonTitle: false,
    courseLessonDescription: false,
    lessonSequenceOrder: false,
    lessonPrerequisites: false,
    lessonObjective: false,
    lessonCompletionTime: false,
    lessonTag: false,
    lessonContentTitle: false,
    lessonContentDescription: false,
    lessonContent: false,
  };

  originalCourseLesson: CourseLesson | null = null;
  originalCourseLessonContent: CourseLessonContent | null = null;

  //An object that stores the values for the course lesson delete dialog
  courseLessonDeleteDialogValues: CourseLessonShorthand = {
    courseLessonId: this.courseLessonId,
    courseId: this.courseId,
    lessonTitle: '',
    lessonTag: '',
    lessonSequenceOrder: 0,
    courseLessonSupplementaryMaterialCount: 0,
    courseLessonSupplementaryMaterialTotalSize: 0,
    createdAt: '',
  };

  //Variables for handling the displaying and styling the message and other related elements to the status of the course creation request to the server and it's subsequent response

  //Flag variable that indicates up to what step of the course creation process the user is currently at
  //If it is null, the course lesson has been create, and the course lesson is being edited
  //0 - indicates that the course lesson is created
  //1 - indicates that the course lesson content has been added to the lesson
  //2 - indicates that the course lesson supplementary materials have been added to the lesson
  currentStepOfLessonCreation: number | null = null;

  //Variables for styling the spinner progress bar
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';

  isDataTransmissionActive: boolean = false;
  isDataTransmissionComplete: boolean = false;

  //Variables for the styling of the create course lesson button
  submitDataButtonText: string = 'Create Lesson';
  submitDataButtonColor: string = 'green';
  submitDataButtonMargin: string = '12px 0px';
  //Variable that holds the message that is displayed above the create course button, this variable is used to notify the user what do they need to do to create a course lesson
  submitDataMessage: string =
    'Add the lesson to the course, then you can add supplementary material';
  submitDataMessageColor: string = 'rgb(2, 80, 176)';

  //Variables for the styling of the save changes button
  updateDataButtonText: string = 'Save changes';
  updateDataButtonColor: string = 'green';
  updateDataButtonMargin: string = '12px 0px';

  //Variables for the styling of the discard changes button
  discardUpdateDataButtonText: string = 'Discard changes';
  discardUpdateDataButtonColor: string = 'red';
  discardUpdateDataButtonMargin: string = '12px 0px';

  //Variables for the styling of the delete lesson button
  deleteDataButtonText: string = 'Delete lesson';
  deleteDataButtonColor: string = 'red';
  deleteDataButtonMargin: string = '12px 0px';

  //Variables for the styling of the go back button
  goBackButtonText: string = 'Go back';
  goBackButtonColor: string = 'blue';
  goBackButtonMargin: string = '12px 0px';

  selectedToServerOperation: FrontendToBackendOperationType =
    FrontendToBackendOperationType.None;

  //Variable that holds the message that is displayed above the create course button, this variable is used to notify the user what do they need to do to create a course lesson
  updateDataMessage: string =
    'Add the lesson to the course, then you can add supplementary material';
  updateDataMessageColor: string = 'rgb(2, 80, 176)';

  deleteDataMessage: string =
    'If you want to delete this lesson, click the delete button below';
  deleteDataMessageColor: string = 'red';

  @ViewChild('fileInput') fileInput!: ElementRef;

  currentNumberOfFiles: number = 0;
  currentSpaceTakenUpInMb: number = 0;

  uploadFileButtonText: string = 'Upload selected file';
  uploadFileButtonColor: string = 'green';

  fileUploadInstructionText: string =
    'Drag and drop your file here or select it to upload';

  fileUploadWarningText: string = '';

  selectedFile: File | null = null;

  fileToUploadName: string = '';
  fileToUploadSizeInMegabytes: number = 0;
  fileToUploadType: string = '';
  fileToUploadFileCategory: string = '';

  fileToUploadUrl: string = '';
  fileUploadResultMessage: string = '';
  fileUploadResultColor: string = 'green';

  downloadFileUrl: any;
  selectedFileToDownload: string | null = null;
  downloadProgress: number = 0;
  downloadProgressMessage: string = '';

  //Variable that is used as a flag to specify if the download is disabled, completed or in progress
  //Disabled - The value is NULL
  //Completed - The value is TRUE
  //In progress - The value is FALSE
  isDownloadComplete: boolean | null = null;

  courseLessonSupplementaryMaterials: CourseLessonSupplementaryMaterial[] = [];

  floatingWarningDeleteBoxTitle: string = '';
  floatingWarningDeleteBoxMessage: string = '';
  floatingWarningDeleteBoxMessageColor: string = '';
  floatingWarningDeleteBoxFileToDeleteIndex: string = '';
  floatingWarningDeleteBoxFileToDeleteName: string = '';
  selectedFileCourseLessonSupplementaryMaterialId: string = '';
  floatingWarningDeleteBoxShow: boolean = false;

  //Variables for the button that confirms the action of the floating warning box
  floatingWarningDeleteBoxConfirmButtonText: string = 'Yes, delete this file!';
  floatingWarningDeleteBoxConfirmButtonColor: string = 'red';

  floatingWarningDeleteBoxCancelButtonText: string =
    "No, don't delete this file!";
  floatingWarningDeleteBoxCancelButtonColor: string = 'blue';

  //Common variables for the floating warning box buttons
  floatingWarningBoxButtonMargin: string = '12px 0px';

  selectedFileCourseMainMaterialId: string = '';

  isDeleteOperationComplete: boolean = false;

  //Flag to determine if the table or the stacked list should be displayed for the uploaded files
  isStackedView: boolean = window.innerWidth < 768;

  //Variable index used to keep track of the card that is selected to be expanded but only in the stacked view
  expandedCardIndex: number | null = null;

  //Variables for the button that shows up when the communication with the server to delete the selected file is completed no matter if it was successful or not
  //This button closes the floating warning box
  closeFloatingBoxButtonText: string = 'Ok, close';
  closeFloatingBoxButtonColor: string = 'blue';

  fileNameFormControl = new FormControl('');
  fileDateFormControl = new FormControl('');

  allowedDocumentFileTypes: string[] = [
    'application/pdf',
    'application/msword',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    'application/vnd.ms-powerpoint',
    'application/vnd.openxmlformats-officedocument.presentationml.presentation',
    'application/vnd.ms-excel',
    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    'text/plain',
    'application/rtf',
  ];

  allowedImageFileTypes: string[] = [
    'image/png',
    'image/jpeg',
    'image/jpg',
    'image/gif',
  ];

  allowedVideoFileTypes: string[] = [
    'video/mp4',
    'video/mpeg',
    'video/quicktime',
    'video/x-msvideo',
    'video/x-ms-wmv',
    'video/x-matroska',
  ];

  allowedArchiveFileTypes: string[] = [
    'application/zip',
    'application/x-rar-compressed',
    'application/x-rar',
  ];

  courseLessonToServerOperationMessage: string = '';
  courseLessonToServerOperationMessageColor: string = 'green';
  courseLessonToServerOperationDialogTitle: string = '';

  courseLessonToServerOperationConfirmButtonText: string = '';
  courseLessonToServerOperationConfirmButtonColor: string = '';

  courseLessonToServerOperationCancelButtonText: string = '';
  courseLessonToServerOperationCancelButtonColor: string = '';

  courseLessonToServerOperationCloseDialogButtonText: string = '';
  courseLessonToServerOperationCloseDialogButtonColor: string = '';

  //Variables for handling the styling of the text input component for the course lesson title
  courseLessonTitleLabel: string = 'Lesson Title';
  courseLessonTitlePlaceholder: string =
    'Enter how you want to name the lesson';
  courseLessonTitleWarning: string = '';

  //Variables for handling the styling of the text input component for the course lesson description
  courseLessonDescriptionLabel: string = 'Lesson Description';
  courseLessonDescriptionPlaceholder: string = 'Describe the lesson in detail';
  courseLessonDescriptionWarning: string = '';

  //Variables for handling the styling of the text input component for the course lesson sequence order
  courseLessonSequenceOrderLabel: string = 'Lesson sequence order';
  courseLessonSequenceOrderPlaceholder: string =
    'Set the order in which the lesson is to be completed';
  courseLessonSequenceOrderWarning: string = '';

  //Variables for handling the styling of the text input component for the lesson prerequisites
  lessonPrerequisitesLabel: string = 'Lesson prerequisites';
  lessonPrerequisitesPlaceholder: string =
    'What do students need to know before they can start this lesson?';
  lessonPrerequisitesWarning: string = '';

  //Variables for handling the styling of the text input component for the lesson objective
  lessonObjectiveLabel: string = 'Lesson objective';
  lessonObjectivePlaceholder: string = 'What is the objective of this lesson?';
  lessonObjectiveWarning: string = '';

  //Variables for handling the styling of the text input component for the lesson completion time
  lessonCompletionTimeLabel: string = 'Lesson completion time';
  lessonCompletionTimePlaceholder: string =
    'How long on average does it take to complete this lesson?';
  lessonCompletionTimeWarning: string = '';

  //Variables for handling the styling of the text input component for the lesson tag
  lessonTagLabel: string = 'Lesson tag';
  lessonTagPlaceholder: string = 'Enter a tag that describes the lesson';
  lessonTagWarning: string = '';

  courseLessonModalFormGroup = new FormGroup({
    courseLessonTitle: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(255),
    ]),
    courseLessonDescription: new FormControl('', [
      Validators.required,
      Validators.minLength(50),
      Validators.maxLength(510),
    ]),
    lessonSequenceOrder: new FormControl(1, [
      Validators.required,
      Validators.min(1),
    ]),
    lessonPrerequisites: new FormControl('', [Validators.maxLength(510)]),
    lessonObjective: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(255),
    ]),
    lessonCompletionTime: new FormControl('', [Validators.required]),
    lessonTag: new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.pattern(/^\w+/g),
    ]),
  });

  //Variables for handling the styling of the text input component for the lesson content title
  lessonContentTitleLabel: string = 'Content title';
  lessonContentTitlePlaceholder: string = 'Enter the title of the content';
  lessonContentTitleWarning: string = '';

  //Variables for handling the styling of the text input component for the lesson content description
  lessonContentDescriptionLabel: string = 'Content description';
  lessonContentDescriptionPlaceholder: string = 'Describe the content';
  lessonContentDescriptionWarning: string = '';
  lessonContentDescriptionTextAreaResizeType: TextAreaResizeType =
    TextAreaResizeType.None;

  //Variables for handling the styling of the text input component for the lesson content
  lessonContentLabel: string = 'Content';
  lessonContentPlaceholder: string = 'Enter the content';
  lessonContentWarning: string = '';
  lessonContentData: string = '';

  lessonContentFormGroup = new FormGroup({
    lessonContentTitle: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(255),
    ]),
    lessonContentDescription: new FormControl('', [
      Validators.required,
      Validators.minLength(100),
      Validators.maxLength(1000),
    ]),
    lessonContent: new FormControl('', [Validators.required]),
  });

  ngOnInit(): void {
    //Check if the course lesson id was provided by the parent component
    if (this.courseLessonId) {
      //If provided, set the course lesson modal into an edit mode and fetch the course data from the backend
      this.isCreateOrEditMode = false;
      this.modalTitle = 'Edit Course Lesson';

      this.getExistingData();
    }
    if (!this.courseLessonId) {
      this.isCreateOrEditMode = true;
      this.modalTitle = 'Create Course Lesson';
    }
    this.courseLessonModalFormGroup.controls.courseLessonTitle.valueChanges.subscribe(
      (value) => {
        this.checkCourseLessonTitle();
      }
    );
    this.courseLessonModalFormGroup.controls.courseLessonDescription.valueChanges.subscribe(
      (value) => {
        this.checkCourseLessonDescription();
      }
    );
    this.courseLessonModalFormGroup.controls.lessonSequenceOrder.valueChanges.subscribe(
      (value) => {
        this.checkCourseLessonSequenceOrder();
      }
    );
    this.courseLessonModalFormGroup.controls.lessonPrerequisites.valueChanges.subscribe(
      (value) => {
        this.checkCourseLessonPrerequisites();
      }
    );
    this.courseLessonModalFormGroup.controls.lessonObjective.valueChanges.subscribe(
      (value) => {
        this.checkCourseLessonObjective();
      }
    );
    this.courseLessonModalFormGroup.controls.lessonCompletionTime.valueChanges.subscribe(
      (value) => {
        this.checkCourseCompletionTime();
      }
    );
    this.courseLessonModalFormGroup.controls.lessonTag.valueChanges.subscribe(
      (value) => {
        this.checkCourseLessonTag();
      }
    );
    this.lessonContentFormGroup.controls.lessonContentTitle.valueChanges.subscribe(
      () => {
        this.checkCourseLessonContentTitle();
      }
    );
    this.lessonContentFormGroup.controls.lessonContentDescription.valueChanges.subscribe(
      () => {
        this.checkCourseLessonContentDescription();
      }
    );

    this.lessonContentFormGroup.controls.lessonContent.valueChanges.subscribe(
      (value) => {
        this.checkCourseLessonContent();
      }
    );
  }

  //Function to call to check the all the fields for errors
  checkForErrors() {
    this.checkCourseLessonTitle();
    this.checkCourseLessonDescription();
    this.checkCourseLessonSequenceOrder();
    this.checkCourseLessonPrerequisites();
    this.checkCourseLessonObjective();
    this.checkCourseCompletionTime();
    this.checkCourseLessonTag();
    this.checkCourseLessonContentTitle();
    this.checkCourseLessonContentDescription();
    this.checkCourseLessonContent();

    if (
      this.courseLessonModalFormGroup.invalid ||
      this.lessonContentFormGroup.invalid
    ) {
      this.submitDataMessage = 'Please remove any errors to proceed!';
      this.submitDataMessageColor = 'red';
      this.updateDataMessageColor = 'red';
      this.updateDataMessage = 'Please remove any errors to proceed!';
    }
    if (
      this.courseLessonModalFormGroup.valid &&
      this.lessonContentFormGroup.valid
    ) {
      this.submitDataMessage = 'All fields are valid!';
      this.submitDataMessageColor = 'green';
      this.updateDataMessageColor = 'green';
      this.updateDataMessage = 'All fields are valid!';
    }
    this.courseLessonModalFormGroup.valueChanges.subscribe(() => {
      console.log('Checking for errors for courseLessonModalFormGroup');
      console.log(
        'Is courseLessonModalFormGroup valid:',
        this.courseLessonModalFormGroup.valid
      );
      console.log(
        'Is lessonContentFormGroup valid:',
        this.lessonContentFormGroup.valid
      );
      if (
        this.courseLessonModalFormGroup.invalid ||
        this.lessonContentFormGroup.invalid
      ) {
        this.submitDataMessage = 'Please remove any errors to proceed!';
        this.submitDataMessageColor = 'red';
        this.updateDataMessageColor = 'red';
        this.updateDataMessage = 'Please remove any errors to proceed!';
      }
      if (
        this.courseLessonModalFormGroup.valid &&
        this.lessonContentFormGroup.valid
      ) {
        this.submitDataMessage = 'All fields are valid!';
        this.submitDataMessageColor = 'green';
        this.updateDataMessageColor = 'green';
        this.updateDataMessage = 'All fields are valid!';
      }
    });
    this.lessonContentFormGroup.valueChanges.subscribe(() => {
      if (
        this.courseLessonModalFormGroup.invalid ||
        this.lessonContentFormGroup.invalid
      ) {
        this.submitDataMessage = 'Please remove any errors to proceed!';
        this.submitDataMessageColor = 'red';
        this.updateDataMessageColor = 'red';
        this.updateDataMessage = 'Please remove any errors to proceed!';
      }
      if (
        this.courseLessonModalFormGroup.valid &&
        this.lessonContentFormGroup.valid
      ) {
        this.submitDataMessage = 'All fields are valid!';
        this.submitDataMessageColor = 'green';
        this.updateDataMessageColor = 'green';
        this.updateDataMessage = 'All fields are valid!';
      }
    });
  }

  //Function to check the course lesson title for errors
  checkCourseLessonTitle() {
    const errors =
      this.courseLessonModalFormGroup.controls.courseLessonTitle.errors;
    console.log('Lesson title errors:', errors);
    if (errors) {
      if (errors['required']) {
        this.courseLessonTitleWarning = 'This field is required';
      }
      if (errors['minlength']) {
        this.courseLessonTitleWarning =
          'Title of the lesson must be at least 10 characters long';
      }
      if (errors['maxlength']) {
        this.courseLessonTitleWarning =
          'Title of the lesson must be at most 255 characters long';
      }
    }
    if (!errors) {
      this.courseLessonTitleWarning = '';
    }
  }

  //Function to check the course lesson description for errors
  checkCourseLessonDescription() {
    const errors =
      this.courseLessonModalFormGroup.controls.courseLessonDescription.errors;
    if (errors) {
      if (errors['required']) {
        this.courseLessonDescriptionWarning = 'This field is required';
      }
      if (errors['minlength']) {
        this.courseLessonDescriptionWarning =
          'Description must be at least 50 characters long';
      }
      if (errors['maxlength']) {
        this.courseLessonDescriptionWarning =
          'Description must be at most 510 characters long';
      }
    }
    if (!errors) {
      this.courseLessonDescriptionWarning = '';
    }
  }

  //Function to call to check the lesson sequence order for errors
  checkCourseLessonSequenceOrder() {
    const errors =
      this.courseLessonModalFormGroup.controls.lessonSequenceOrder.errors;
    if (errors) {
      if (errors['required']) {
        this.courseLessonSequenceOrderWarning = 'This field is required';
      }
      if (errors['min']) {
        this.courseLessonSequenceOrderWarning = 'Minimum value is 1';
      }
    }
    if (!errors) {
      this.courseLessonSequenceOrderWarning = '';
    }
  }

  //Function to call to check the lesson prerequisites for errors
  checkCourseLessonPrerequisites() {
    const errors =
      this.courseLessonModalFormGroup.controls.lessonPrerequisites.errors;
    console.log('Lesson prerequisites errors:', errors);
    if (errors) {
      if (errors['maxlength']) {
        this.lessonPrerequisitesWarning =
          'Prerequisites must be at most 510 characters long';
      }
    }
    if (!errors) {
      this.lessonPrerequisitesWarning = '';
    }
  }

  //Function to check the course lesson objective for errors
  checkCourseLessonObjective() {
    const errors =
      this.courseLessonModalFormGroup.controls.lessonObjective.errors;
    if (errors) {
      if (errors['required']) {
        this.lessonObjectiveWarning = 'This field is required';
      }
      if (errors['minlength']) {
        this.lessonObjectiveWarning =
          'Lesson objective must be at least 10 characters long';
      }
      if (errors['maxlength']) {
        this.lessonObjectiveWarning =
          'Lesson objective must be at most 255 characters long';
      }
    }
    if (!errors) {
      this.lessonObjectiveWarning = '';
    }
  }

  //Function to call to check the lesson completion time for errors
  checkCourseCompletionTime() {
    const errors =
      this.courseLessonModalFormGroup.controls.lessonCompletionTime.errors;
    console.log('Lesson completion time errors:', errors);
    if (errors) {
      this.lessonCompletionTimeWarning = 'This field is required';
    }
    if (!errors) {
      this.lessonCompletionTimeWarning = '';
    }
  }
  //Function to call to check the lesson tag for errors
  checkCourseLessonTag() {
    const errors = this.courseLessonModalFormGroup.controls.lessonTag.errors;
    if (errors) {
      if (errors['required']) {
        this.lessonTagWarning = 'This field is required';
      }
      if (errors['minlength']) {
        this.lessonTagWarning =
          'Lesson tag must be at least 10 characters long';
      }
      if (errors['pattern']) {
        this.lessonTagWarning =
          'Lesson tag cannot contain whitespaces, special characters or #';
      }
    }
    if (!errors) {
      this.lessonTagWarning = '';
    }
  }

  checkCourseLessonContentTitle() {
    const errors =
      this.lessonContentFormGroup.controls.lessonContentTitle.errors;

    if (errors) {
      if (errors['required']) {
        this.lessonContentTitleWarning = 'This field is required';
      }
      if (errors['minlength']) {
        this.lessonContentTitleWarning =
          'Content title must be at least 10 characters long';
      }
      if (errors['maxlength']) {
        this.lessonContentTitleWarning =
          'Content title can be only up to 255 characters long';
      }
    }
    if (!errors) {
      this.lessonContentTitleWarning = '';
    }
  }

  checkCourseLessonContentDescription() {
    const errors =
      this.lessonContentFormGroup.controls.lessonContentDescription.errors;
    if (errors) {
      if (errors['required']) {
        this.lessonContentDescriptionWarning = 'This field is required';
      }
      if (errors['minlength']) {
        this.lessonContentDescriptionWarning =
          'Content description must be at least 100 characters long';
      }
      if (errors['maxlength']) {
        this.lessonContentDescriptionWarning =
          'Content description can only be up to 1000 characters long';
      }
    }
    if (!errors) {
      this.lessonContentDescriptionWarning = '';
    }
  }

  checkCourseLessonContent() {
    //Get the value of the lessonContent form control from the lessonContentFormGroup form group
    const lessonContent =
      this.lessonContentFormGroup.controls.lessonContent.value;

    ///Remove HTML tags from the lesson content
    const lessonContentWithoutHtml = lessonContent?.replace(
      /<\/?[^>]+(>|$)/g,
      ''
    );

    //Check if the lessonContent is empty or null, with the removed HTML tags
    const isEmpty = !lessonContent || lessonContentWithoutHtml?.trim() === '';

    ///Set the lessonContent form control to required if the lessonContent is empty or null
    if (isEmpty) {
      this.lessonContentFormGroup.controls.lessonContent.setErrors({
        required: true,
      });
    }

    //Check if the lessonContent is empty or null, with the removed HTML tags
    const errors = this.lessonContentFormGroup.controls.lessonContent.errors;
    if (errors) {
      if (errors['required']) {
        this.lessonContentWarning = 'This field is required';
      }
    }
    if (!errors) {
      this.lessonContentWarning = '';
    }
  }

  toggleCloseModal() {
    this.closeModal.emit();
  }

  submitCourseLessonAndCourseLessonContent() {
    this.checkForErrors();
    if (
      this.courseLessonModalFormGroup.invalid ||
      this.lessonContentFormGroup.invalid
    ) {
      return;
    }

    //Check if the form is valid
    if (this.courseLessonModalFormGroup.valid) {
      //Check whatever the modal is in create or edit mode
      this.createCourseLesson();
    }
  }
  //Function to send request to the backend to create a new course lesson
  createCourseLesson() {
    //Check if the form is valid
    if (this.courseLessonModalFormGroup.invalid) {
      return;
    }

    //Get the data from the form group and convert it to a corresponding data type with formatting

    const lessonTitle: string = this.courseLessonModalFormGroup.controls
      .courseLessonTitle.value as string;

    const lessonDescription: string = this.courseLessonModalFormGroup.controls
      .courseLessonDescription.value as string;

    const lessonSequenceOrder: number = this.courseLessonModalFormGroup.controls
      .lessonSequenceOrder.value as number;

    const lessonPrerequisites: string = this.courseLessonModalFormGroup.controls
      .lessonPrerequisites.value as string;

    const lessonObjective: string = this.courseLessonModalFormGroup.controls
      .lessonObjective.value as string;

    //Convert the lessonCompletionTime which value is in string of format HH:MM to a number that represents the total minutes of the lesson
    const lessonCompletionTimeFormatted: string = this
      .courseLessonModalFormGroup.controls.lessonCompletionTime.value as string;

    const [hours, minutes] = lessonCompletionTimeFormatted
      ?.split(':')
      .map(Number);

    const lessonCompletionTime: number = hours * 60 + minutes;

    const lessonTag: string = this.courseLessonModalFormGroup.controls.lessonTag
      .value as string;

    this.isDataTransmissionActive = true;
    this.isDataTransmissionComplete = false;
    this.submitDataMessage = 'Adding lesson to the course...';
    this.submitDataButtonColor = 'blue';

    this.courseCreateService
      .createCourseLesson(
        this.courseId,
        lessonTitle,
        lessonDescription,
        lessonSequenceOrder,
        lessonPrerequisites,
        lessonObjective,
        lessonCompletionTime,
        lessonTag
      )
      .subscribe((response) => {
        console.log(response);
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;

        if (response.success === 'true') {
          this.submitDataMessage =
            'Lesson added to the course, adding lesson content...';
          this.submitDataButtonColor = 'blue';
          this.courseLessonId = response.data.courseLesson.courseLessonId;
          this.createCourseLessonContent();
        }
        if (response.success !== 'true') {
          this.submitDataMessage =
            'Failed to add lesson to the course, ' + response.message;
          this.submitDataMessageColor = 'red';
        }
      });
  }

  createCourseLessonContent() {
    //Check if the form is invalid, and if it is return the function
    if (this.lessonContentFormGroup.invalid) {
      return;
    }

    //Get the data from the form group and convert it to a corresponding data type with formatting

    const lessonContentTitle: string = this.lessonContentFormGroup.controls
      .lessonContentTitle.value as string;

    const lessonContentDescription: string = this.lessonContentFormGroup
      .controls.lessonContentDescription.value as string;

    const lessonContent: string = this.lessonContentFormGroup.controls
      .lessonContent.value as string;

    this.isDataTransmissionActive = true;
    this.isDataTransmissionComplete = false;
    this.courseCreateService
      .createCourseLessonContent(
        this.courseLessonId,
        lessonContentTitle,
        lessonContentDescription,
        lessonContent
      )
      .subscribe((response) => {
        console.log(response);
        if (response.success === 'true') {
          this.isDataTransmissionActive = false;
          this.isDataTransmissionComplete = true;
          this.submitDataMessage =
            'Lesson content added to the lesson, you can now upload supplementary materials to the lesson!';
          this.submitDataButtonColor = 'green';
          this.showSupplementaryMaterialUpload = true;
        }
      });
  }

  onFileSelected(event: any) {
    console.log('File selected event: ', event);
    console.log('Selected file name: ', event.name);

    this.selectedFile = event;
    this.fileInput.nativeElement = event;
    this.fileUploadResultMessage = '';

    console.log('Selected file:', this.selectedFile);
    console.log('File input value:', this.fileInput.nativeElement.value);
    if (this.selectedFile) {
      //Check if the file type is allowed
      if (!this.validateFileType(this.selectedFile)) {
        console.log('File type not allowed');
        this.fileUploadWarningText =
          'This file type is not allowed, choose another file';
        this.fileUploadInstructionText = '';
        this.fileToUploadUrl = '';
        return;
      }

      //Check if the file size is allowed
      if (!this.validateFileSize(this.selectedFile)) {
        console.log('File size exceeds limit');
        this.fileUploadWarningText = `File size of ${
          Math.round((this.selectedFile.size / (1024 * 1024)) * 1e2) / 1e2
        } MB exceeds limit, max size for ${this.getFileCategory(
          this.selectedFile.type
        ).toLowerCase()}s is ${this.getFileSizeLimit(
          this.selectedFile.type
        )}MB`;
        this.fileUploadInstructionText = '';
        this.fileToUploadUrl = '';
        return;
      }

      this.fileUploadInstructionText =
        'File selected, click button below to upload';
      this.fileToUploadName = this.selectedFile.name;
      this.fileToUploadSizeInMegabytes =
        Math.round((this.selectedFile.size / (1024 * 1024)) * 1e2) / 1e2;
      this.fileToUploadFileCategory = this.getFileCategory(
        this.selectedFile.type
      );
      this.fileToUploadType = this.getCleanFileType(this.selectedFile.type);

      if (this.isPreviewableFileType(this.selectedFile.type)) {
        this.fileToUploadUrl = URL.createObjectURL(this.selectedFile);
        console.log('File URL:', this.fileToUploadUrl);
      } else {
        this.fileToUploadUrl = '';
      }
    }
  }

  isPreviewableFileType(fileType: string): boolean {
    const previewebleTypes = ['application/pdf', ...this.allowedImageFileTypes];
    return previewebleTypes.includes(fileType);
  }
  getFileCategory(fileType: string): string {
    if (this.allowedImageFileTypes.includes(fileType)) {
      return 'Image';
    }
    if (this.allowedVideoFileTypes.includes(fileType)) {
      return 'Video';
    }
    if (this.allowedDocumentFileTypes.includes(fileType)) {
      return 'Document';
    }
    if (this.allowedArchiveFileTypes.includes(fileType)) {
      return 'Archive';
    }
    return 'Unknown';
  }

  getCleanFileType(fileType: string): string {
    let cleanType = '';

    cleanType = fileType.split('/').pop() || '';
    cleanType = cleanType
      .replace('vnd.openxmlformats-officedocument.', '')
      .replace('vnd.', '')
      .replace('x-', '')
      .replace('ms-', '')
      .replace('application/', '')
      .replace('text/', '');

    return cleanType;
  }

  getFileSizeLimit(fileType: string): number {
    if (this.allowedImageFileTypes.includes(fileType)) {
      return 5;
    }
    if (this.allowedDocumentFileTypes.includes(fileType)) {
      return 10;
    }
    if (this.allowedVideoFileTypes.includes(fileType)) {
      return 100;
    }
    if (this.allowedArchiveFileTypes.includes(fileType)) {
      return 100;
    }
    return 0;
  }

  validateFileType(file: File): boolean {
    const fileType = file.type;

    return (
      this.allowedDocumentFileTypes.includes(fileType) ||
      this.allowedImageFileTypes.includes(fileType) ||
      this.allowedVideoFileTypes.includes(fileType) ||
      this.allowedArchiveFileTypes.includes(fileType)
    );
  }
  validateFileSize(file: File): boolean {
    const fileSizeInMB = file.size / (1024 * 1024);
    const fileType = file.type;

    if (this.allowedImageFileTypes.includes(fileType)) {
      return fileSizeInMB <= 5;
    }

    if (this.allowedDocumentFileTypes.includes(fileType)) {
      return fileSizeInMB <= 10;
    }

    if (this.allowedVideoFileTypes.includes(fileType)) {
      return fileSizeInMB <= 100;
    }

    if (this.allowedArchiveFileTypes.includes(fileType)) {
      return fileSizeInMB <= 50;
    }

    return false;
  }

  uploadFile() {
    if (this.selectedFile) {
      this.fileUploadInstructionText = '';
      this.isDataTransmissionActive = true;
      this.fileUploadResultMessage = 'Uploading file...';
      this.fileUploadResultColor = 'blue';

      const givenFileName =
        this.fileNameFormControl.value !== '' &&
        this.fileNameFormControl.value !== null
          ? this.fileNameFormControl.value
          : this.selectedFile.name;

      const givenDateTimeOfCreation =
        this.fileDateFormControl.value !== '' &&
        this.fileDateFormControl.value !== null
          ? new Date(this.fileDateFormControl.value).getTime().toString()
          : Date.now().toString();

      console.log('Given File Name: ', givenFileName);
      console.log('Given File Date: ', givenDateTimeOfCreation);
      console.log(
        'Given File Date in UNIX: ',
        new Date(givenDateTimeOfCreation).getTime()
      );
      this.isDataTransmissionActive = true;

      this.courseCreateService
        .uploadFileAsCourseLessonSupplementaryMaterial(
          this.courseLessonId,
          givenFileName,
          givenDateTimeOfCreation,
          this.selectedFile
        )
        .subscribe({
          //Handle successful response or still ongoing upload
          next: (result) => {
            //Check if the result is a type of number, if it is, then the upload is still in progress
            if (typeof result === 'number') {
              console.log(`Upload progress: ${result}%`);
              this.fileUploadResultMessage = `Uploaded: ${result}%`;
            } else {
              this.isDataTransmissionActive = false;
              this.fileUploadResultMessage = 'File uploaded successfully';
              this.fileUploadResultColor = 'green';
              console.log('Upload complete', result);
              this.selectedFile = null;
              URL.revokeObjectURL(this.fileToUploadUrl);
              this.fileToUploadUrl = '';
              this.loadCourseLessonSupplementaryMaterials();
              setTimeout(() => {
                this.fileUploadResultMessage = '';
                this.fileUploadInstructionText =
                  'Drag and drop your file here or select it to upload';
              }, 5000);
            }
          },
          //Handle error response
          error: (error) => {
            this.isDataTransmissionActive = false;
            console.log('Upload failed', error);
            this.fileUploadResultColor = 'red';
            this.fileUploadResultMessage = 'Upload failed, ' + error.message;
            this.selectedFile = null;
            this.loadCourseLessonSupplementaryMaterials();
          },
        });
    }
  }

  getFileSizeInMb(fileSizeInBytes: string | number): number {
    const fileSizeInBytesInt =
      typeof fileSizeInBytes === 'string'
        ? parseInt(fileSizeInBytes)
        : fileSizeInBytes;
    return (Math.round(fileSizeInBytesInt / (1024 * 1024)) * 1e2) / 1e2;
  }

  getDateTimeFromUnixMillis(timestamp: number | string): string {
    const date = new Date(timestamp);
    const day = date.getDate();
    const month = date.getMonth() + 1;
    const year = date.getFullYear();
    const hours = date.getHours();
    const minutes = date.getMinutes();
    const seconds = date.getSeconds();
    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
  }

  getFileExtension(fileType: string): string {
    let extension = fileType.split('/').pop() || '';

    // Handle special cases for Office documents
    if (extension.includes('wordprocessingml.document')) {
      return 'docx';
    }
    if (extension.includes('presentationml.presentation')) {
      return 'pptx';
    }
    if (extension.includes('spreadsheetml.sheet')) {
      return 'xlsx';
    }

    // Clean up common prefixes
    extension = extension
      .replace('vnd.openxmlformats-officedocument.', '')
      .replace('vnd.', '')
      .replace('x-', '')
      .replace('ms-', '')
      .replace('application/', '')
      .replace('text/', '');

    return extension;
  }

  handleFileInput(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.onFileSelected(input.files[0]);
    }
  }

  //Functions for handling file drag and drop

  //Handle dragover event
  onDragOver(event: DragEvent) {
    event.preventDefault();
    const dropzone = event.target as HTMLElement;
    dropzone.classList.add('dragover');
  }

  //Handle dragleave event
  onDragLeave(event: DragEvent) {
    const dropzone = event.target as HTMLElement;
    dropzone.classList.remove('dragover');
  }

  //Handle drop event
  onDrop(event: DragEvent) {
    event.preventDefault();
    const dropzone = event.target as HTMLElement;
    dropzone.classList.remove('dragover');

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      this.onFileSelected(event.dataTransfer.files[0]);
    }
  }

  loadCourseLessonSupplementaryMaterials() {
    if (this.courseLessonId) {
      this.courseCreateService
        .getCourseLessonSupplementaryMaterialsByCourseLessonId(
          this.courseLessonId
        )
        .subscribe({
          next: (response) => {
            console.log(response);
            if (response.success === 'true') {
              this.courseLessonSupplementaryMaterials =
                response.data.courseLessonSupplementaryMaterials;
              this.currentNumberOfFiles =
                this.courseLessonSupplementaryMaterials.length;
              this.currentSpaceTakenUpInMb = this.getFileSizeInMb(
                response.data.courseLessonSupplementaryMaterialsSize
              );
            }
            if (response.success !== 'true') {
              this.courseLessonSupplementaryMaterials = [];
              this.currentNumberOfFiles = 0;
              this.currentSpaceTakenUpInMb = 0;
            }
          },
          error: (error) => {
            console.log(error);
          },
        });
    }
  }

  downloadCourseLessonSupplementaryMaterial(
    courseLessonSupplementaryMaterialId: string
  ) {
    this.downloadFileUrl = null;
    console.log('Downloading file:', courseLessonSupplementaryMaterialId);

    this.selectedFileToDownload = this.courseLessonSupplementaryMaterials.find(
      (fileToDownload) => {
        return (
          fileToDownload.courseLessonSupplementaryMaterialId ===
          courseLessonSupplementaryMaterialId
        );
      }
    )?.fileName as string;

    console.log('Selected file to download:', this.selectedFileToDownload);
    this.courseCreateService
      .downloadCourseLessonSupplementaryMaterialByCourseLessonSupplementaryMaterialId(
        courseLessonSupplementaryMaterialId
      )
      .subscribe({
        next: (progressOrFile) => {
          if (typeof progressOrFile === 'number') {
            this.downloadProgress = progressOrFile;
            this.downloadProgressMessage = `${this.selectedFileToDownload} downloading ${this.downloadProgress}%`;
            this.isDownloadComplete = false;
          } else if (progressOrFile instanceof Blob) {
            this.downloadProgressMessage = 'Download complete';
            this.downloadProgress = 100;

            const url = window.URL.createObjectURL(progressOrFile);
            window.open(url);
            this.downloadFileUrl = (
              this.sanitizer.bypassSecurityTrustResourceUrl(url) as any
            ).changingThisBreaksApplicationSecurity;
            console.log('Downloaded file url:', this.downloadFileUrl);
            this.isDownloadComplete = true;
          }
        },
        error: (error) => {
          console.error('Error downloading file:', error);
          this.downloadProgressMessage =
            'Error downloading file, ' + error.message;
          this.isDownloadComplete = false;
        },
      });
  }

  deleteCourseLessonSupplementaryMaterial(
    courseLessonSupplementaryMaterialId: string,
    index: number
  ) {
    console.log(
      'Deleting course lesson supplementary material:',
      courseLessonSupplementaryMaterialId
    );

    this.floatingWarningDeleteBoxTitle = 'Delete file';
    this.floatingWarningDeleteBoxMessage =
      'Are you sure you want to delete this file?';
    this.floatingWarningDeleteBoxMessageColor = 'red';
    this.floatingWarningDeleteBoxFileToDeleteIndex = `File position in the list: ${
      index + 1
    }`;
    this.floatingWarningDeleteBoxFileToDeleteName = `File name: ${this.courseLessonSupplementaryMaterials[index].fileName}`;
    this.selectedFileCourseLessonSupplementaryMaterialId =
      courseLessonSupplementaryMaterialId;
    this.floatingWarningDeleteBoxConfirmButtonText = 'Yes, delete this file!';
    this.floatingWarningDeleteBoxConfirmButtonColor = 'red';

    this.floatingWarningDeleteBoxCancelButtonText =
      "No, don't delete this file!";
    this.floatingWarningDeleteBoxCancelButtonColor = 'blue';
    this.floatingWarningDeleteBoxShow = true;
  }

  confirmDelete() {
    if (this.selectedFileCourseLessonSupplementaryMaterialId) {
      this.courseCreateService
        .deleteCourseLessonSupplementaryMaterialByCourseLessonSupplementaryMaterialId(
          this.selectedFileCourseLessonSupplementaryMaterialId
        )
        .subscribe((response) => {
          console.log(response);
          this.isDeleteOperationComplete = true;
          this.loadCourseLessonSupplementaryMaterials();
          if (response.success === 'true') {
            this.floatingWarningDeleteBoxMessage = 'File deleted successfully';
            this.floatingWarningDeleteBoxMessageColor = 'green';
            this.floatingWarningDeleteBoxFileToDeleteIndex = '';
            this.floatingWarningDeleteBoxFileToDeleteName = '';
          }
          if (response.success === 'false') {
            this.floatingWarningDeleteBoxMessage = 'Failed to delete file:';
            this.floatingWarningDeleteBoxMessageColor = 'red';
            this.floatingWarningDeleteBoxFileToDeleteIndex = response.message;
            this.floatingWarningDeleteBoxFileToDeleteName = '';
          }
        });
    }
  }

  closeWarningFloatingBox() {
    this.floatingWarningDeleteBoxShow = false;
    this.isDeleteOperationComplete = false;
  }

  toggleCardExpansion(index: number) {
    console.log('Toggle card expansion for index:', index);
    this.expandedCardIndex = this.expandedCardIndex === index ? null : index;
  }

  getExistingData() {
    if (!this.isCreateOrEditMode) {
      console.log('Fetching existing data...');
      this.courseCreateService
        .getCourseLessonWithContentAndSupplementaryMaterialsByCourseLessonId(
          this.courseLessonId
        )
        .subscribe((response) => {
          if (response.success === 'true') {
            const existingCourseLesson: CourseLesson =
              response.data.courseLesson.courseLesson;
            const existingCourseLessonContent: CourseLessonContent =
              response.data.courseLesson.courseLessonContent;
            this.courseLessonSupplementaryMaterials =
              response.data.courseLesson.courseLessonSupplementaryMaterials;

            this.courseLessonModalFormGroup.setValue({
              courseLessonTitle: existingCourseLesson.lessonTitle,
              courseLessonDescription: existingCourseLesson.lessonDescription,
              lessonSequenceOrder: existingCourseLesson.lessonSequenceOrder,
              lessonPrerequisites: existingCourseLesson.lessonPrerequisites,
              lessonObjective: existingCourseLesson.lessonObjective,
              lessonCompletionTime: this.getHoursAndMinutesFromMinutes(
                existingCourseLesson.lessonCompletionTimeInMinutes
              ),
              lessonTag: existingCourseLesson.lessonTag,
            });

            this.lessonContentFormGroup.setValue({
              lessonContentTitle: existingCourseLessonContent.title,
              lessonContentDescription: existingCourseLessonContent.description,
              lessonContent: existingCourseLessonContent.contentData,
            });

            this.showSupplementaryMaterialUpload = true;
            this.submitDataMessage = `${this.courseLessonSupplementaryMaterials.length} supplementary materials.`;

            //Assign the data that was retrieved from the database to the originalCourseLesson object, to be later used to check if the user has made any changes to the data.
            this.originalCourseLesson = {
              courseLessonId: this.courseLessonId,
              courseId: existingCourseLesson.courseId,
              lessonTitle: existingCourseLesson.lessonTitle,
              lessonDescription: existingCourseLesson.lessonDescription,
              lessonSequenceOrder: existingCourseLesson.lessonSequenceOrder,
              lessonPrerequisites: existingCourseLesson.lessonPrerequisites,
              lessonObjective: existingCourseLesson.lessonObjective,
              lessonCompletionTimeInMinutes:
                existingCourseLesson.lessonCompletionTimeInMinutes,
              lessonTag: existingCourseLesson.lessonTag,
              createdAt: existingCourseLesson.createdAt,
            };

            console.log('Existing course lesson:', this.originalCourseLesson);
            //Assign the data that was retrieved from the database to the originalCourseLessonContent object, to be later used to check if the user has made any changes to the data.
            this.originalCourseLessonContent = {
              courseLessonContentId:
                existingCourseLessonContent.courseLessonContentId,
              courseLessonId: existingCourseLessonContent.courseLessonId,
              title: existingCourseLessonContent.title,
              description: existingCourseLessonContent.description,
              contentData: existingCourseLessonContent.contentData,
            };
          }
        });
    }
  }

  getHoursAndMinutesFromMinutes(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const remainingMinutes = minutes % 60;

    // Ensure two-digit formatting
    const formattedHours = hours.toString().padStart(2, '0');
    const formattedMinutes = remainingMinutes.toString().padStart(2, '0');

    return `${formattedHours}:${formattedMinutes}`;
  }

  toggleDialog() {
    this.showDialog = !this.showDialog;
  }

  toggleSaveDialog() {
    //Check for errors
    this.checkForErrors();
    console.log(
      'Title value: ',
      this.courseLessonModalFormGroup.controls.courseLessonTitle.value
    );
    if (
      this.courseLessonModalFormGroup.invalid ||
      this.lessonContentFormGroup.invalid
    ) {
      return;
    }

    //Check if any of the values have been updated
    if (!this.checkForUpdates()) {
      this.updateDataMessage = 'Updating not possible, no changes were made!';
      this.updateDataMessageColor = 'red';
      return;
    }

    console.log('Toggle save dialog');
    this.courseLessonToServerOperationDialogTitle = 'Save changes';
    this.courseLessonToServerOperationMessage =
      'Are you sure you want to save changes?';
    this.courseLessonToServerOperationMessageColor = 'blue';

    //Set the button texts and colors
    this.courseLessonToServerOperationConfirmButtonText = 'Yes, save changes!';
    this.courseLessonToServerOperationConfirmButtonColor = 'green';
    this.courseLessonToServerOperationCancelButtonText =
      "No, don't save the changes!";
    this.courseLessonToServerOperationCancelButtonColor = 'blue';

    this.courseLessonToServerOperationCloseDialogButtonText =
      'Ok, close dialog';
    this.courseLessonToServerOperationCloseDialogButtonColor = 'orange';

    //Set the selected operation as Update
    this.selectedToServerOperation = FrontendToBackendOperationType.Update;

    this.toggleDialog();
  }

  toggleDiscardDialog() {
    //Check if any of the values have been updated
    if (!this.checkForUpdates()) {
      this.updateDataMessage = 'No changes have been made, no need to discard!';
      this.updateDataMessageColor = 'red';
      return;
    }

    this.courseLessonToServerOperationDialogTitle = 'Discard changes';
    this.courseLessonToServerOperationMessage =
      'Are you sure you want to discard changes?';
    this.courseLessonToServerOperationMessageColor = 'red';

    //Set the button texts and colors
    this.courseLessonToServerOperationConfirmButtonText =
      'Yes, discard changes!';
    this.courseLessonToServerOperationConfirmButtonColor = 'red';
    this.courseLessonToServerOperationCancelButtonText =
      "No, don't save discard the changes!";
    this.courseLessonToServerOperationCancelButtonColor = 'blue';

    this.courseLessonToServerOperationCloseDialogButtonText =
      'Ok, close dialog';
    this.courseLessonToServerOperationCloseDialogButtonColor = 'orange';

    //Set the selected operation as Update
    this.selectedToServerOperation = FrontendToBackendOperationType.Discard;
    this.toggleDialog();
  }

  //Used for confirming the operation to the server
  //Operations can be Create and Update
  confirmToServerOperation() {
    //Check if the selected operation is Update
    if (
      this.selectedToServerOperation === FrontendToBackendOperationType.Update
    ) {
      this.updateCourseLesson();
    }

    if (
      this.selectedToServerOperation === FrontendToBackendOperationType.Discard
    ) {
      this.discardChangesToTheCourseLesson();
    }
  }

  //Method for sending a request to the server to update a data for the CourseLesson and CourseLessonContent
  updateCourseLesson() {
    console.log('Update lesson data');

    const lessonCompletionTimeFormatted: string = this
      .courseLessonModalFormGroup.controls.lessonCompletionTime.value as string;
    const [hours, minutes] = lessonCompletionTimeFormatted
      ?.split(':')
      .map(Number);

    const lessonCompletionTime: number = hours * 60 + minutes;
    const updateRequest: CourseLessonUpdateRequest = {
      courseLessonId: this.courseLessonId,
      lessonTitle: this.courseLessonModalFormGroup.controls.courseLessonTitle
        .value as string,
      updateLessonTitle: this.updatedCourseLessonValues.courseLessonTitle,
      lessonDescription: this.courseLessonModalFormGroup.controls
        .courseLessonDescription.value as string,
      updateLessonDescription:
        this.updatedCourseLessonValues.courseLessonDescription,
      lessonSequenceOrder:
        this.courseLessonModalFormGroup.controls.lessonSequenceOrder.value ?? 0,
      updateLessonSequenceOrder:
        this.updatedCourseLessonValues.lessonSequenceOrder,
      lessonPrerequisites: this.courseLessonModalFormGroup.controls
        .lessonPrerequisites.value as string,
      updateLessonPrerequisites:
        this.updatedCourseLessonValues.lessonPrerequisites,
      lessonObjective: this.courseLessonModalFormGroup.controls.lessonObjective
        .value as string,
      updateLessonObjective: this.updatedCourseLessonValues.lessonObjective,
      lessonCompletionTimeInMinutes: lessonCompletionTime,
      updateLessonCompletionTimeInMinutes:
        this.updatedCourseLessonValues.lessonCompletionTime,
      lessonTag: this.courseLessonModalFormGroup.controls.lessonTag
        .value as string,
      updateLessonTag: this.updatedCourseLessonValues.lessonTag,
      lessonContentTitle: this.lessonContentFormGroup.controls
        .lessonContentTitle.value as string,
      updateLessonContentTitle:
        this.updatedCourseLessonValues.lessonContentTitle,
      lessonContentDescription: this.lessonContentFormGroup.controls
        .lessonContentDescription.value as string,
      updateLessonContentDescription:
        this.updatedCourseLessonValues.lessonContentDescription,
      lessonContentData: this.lessonContentFormGroup.controls.lessonContent
        .value as string,
      updateLessonContentData: this.updatedCourseLessonValues.lessonContent,
    };

    console.log('Update request: ', updateRequest);
    this.courseCreateService
      .updateCourseLessonByCourseLessonId(updateRequest)
      .subscribe((response) => {
        console.log(response);
        this.isDataTransmissionActive = true;
        this.isDataTransmissionComplete = false;
        this.courseLessonToServerOperationMessage = 'Updating data...';

        if (response.success === 'true') {
          this.isDataTransmissionActive = false;
          this.isDataTransmissionComplete = true;
          this.courseLessonToServerOperationMessage =
            'Data updated successfully!';
          this.courseLessonToServerOperationMessageColor = 'green';
        }
        if (response.success !== 'true') {
          this.isDataTransmissionActive = false;
          this.isDataTransmissionComplete = true;
          this.courseLessonToServerOperationMessage =
            'Failed to update, ' + response.message;
          this.courseLessonToServerOperationMessageColor = 'red';
        }
      });
  }

  discardChangesToTheCourseLesson() {
    this.courseLessonModalFormGroup.setValue({
      courseLessonTitle: this.originalCourseLesson?.lessonTitle as string,
      courseLessonDescription: this.originalCourseLesson
        ?.lessonDescription as string,
      lessonSequenceOrder: this.originalCourseLesson
        ?.lessonSequenceOrder as number,
      lessonPrerequisites: this.originalCourseLesson
        ?.lessonPrerequisites as string,
      lessonObjective: this.originalCourseLesson?.lessonObjective as string,
      lessonCompletionTime: this.getHoursAndMinutesFromMinutes(
        this.originalCourseLesson?.lessonCompletionTimeInMinutes as number
      ),
      lessonTag: this.originalCourseLesson?.lessonTag as string,
    });

    this.lessonContentFormGroup.setValue({
      lessonContentTitle: this.originalCourseLessonContent?.title as string,
      lessonContentDescription: this.originalCourseLessonContent
        ?.description as string,
      lessonContent: this.originalCourseLessonContent?.contentData as string,
    });

    this.toggleDialog();
  }
  //Method for checking if any of the values have been updated
  checkForUpdates() {
    //Check if the lesson title has been updated
    if (
      this.originalCourseLesson?.lessonTitle !==
      this.courseLessonModalFormGroup.controls.courseLessonTitle.value
    ) {
      this.updatedCourseLessonValues.courseLessonTitle = true;
    }

    //Check if the lesson description has been updated
    if (
      this.originalCourseLesson?.lessonDescription !==
      this.courseLessonModalFormGroup.controls.courseLessonDescription.value
    ) {
      this.updatedCourseLessonValues.courseLessonDescription = true;
    }

    //Check if the lesson sequence order has been updated
    if (
      this.originalCourseLesson?.lessonSequenceOrder !==
      this.courseLessonModalFormGroup.controls.lessonSequenceOrder.value
    ) {
      this.updatedCourseLessonValues.lessonSequenceOrder = true;
    }

    //Check if the lesson prerequisites has been updated
    if (
      this.originalCourseLesson?.lessonPrerequisites !==
      this.courseLessonModalFormGroup.controls.lessonPrerequisites.value
    ) {
      this.updatedCourseLessonValues.lessonPrerequisites = true;
    }

    //Check if the lesson objective has been updated
    if (
      this.originalCourseLesson?.lessonObjective !==
      this.courseLessonModalFormGroup.controls.lessonObjective.value
    ) {
      this.updatedCourseLessonValues.lessonObjective = true;
    }
    const lessonCompletionTimeFormatted: string = this
      .courseLessonModalFormGroup.controls.lessonCompletionTime.value as string;
    const [hours, minutes] = lessonCompletionTimeFormatted
      ?.split(':')
      .map(Number);

    const lessonCompletionTime: number = hours * 60 + minutes;
    //Check if the lesson duration has been updated
    if (
      this.originalCourseLesson?.lessonCompletionTimeInMinutes !==
      lessonCompletionTime
    ) {
      this.updatedCourseLessonValues.lessonCompletionTime = true;
    }

    //Check if the lesson tag has been updated
    if (
      this.originalCourseLesson?.lessonTag !==
      this.courseLessonModalFormGroup.controls.lessonTag.value
    ) {
      this.updatedCourseLessonValues.lessonTag = true;
    }

    //Check if the lesson content title has been updated
    if (
      this.originalCourseLessonContent?.title !==
      this.lessonContentFormGroup.controls.lessonContentTitle.value
    ) {
      this.updatedCourseLessonValues.lessonContentTitle = true;
    }

    //Check if the lesson content description has been updated
    if (
      this.originalCourseLessonContent?.description !==
      this.lessonContentFormGroup.controls.lessonContentDescription.value
    ) {
      this.updatedCourseLessonValues.lessonContentDescription = true;
    }

    //Check if the lesson content has been updated
    if (
      this.originalCourseLessonContent?.contentData !==
      this.lessonContentFormGroup.controls.lessonContent.value
    ) {
      this.updatedCourseLessonValues.lessonContent = true;
    }

    console.log('Updated course lesson values', this.updatedCourseLessonValues);

    const updatedValue = Object.values(this.updatedCourseLessonValues).some(
      (value) => value === true
    );
    console.log('Were any values updated?', updatedValue);
    for (let key in this.updatedCourseLessonValues) {
      console.log(key, this.updatedCourseLessonValues[key]);
      if (this.updatedCourseLessonValues[key] === true) {
        return true;
      }
    }
    return false;
  }

  closeDialog() {
    this.isDataTransmissionActive = false;
    this.isDataTransmissionComplete = false;
    this.courseLessonToServerOperationMessage = '';
    this.courseLessonToServerOperationMessageColor = '';
    this.toggleDialog();
  }
  closeAllDialogs() {
    this.isDataTransmissionActive = false;
    this.isDataTransmissionComplete = false;
    this.courseLessonToServerOperationMessage = '';
    this.courseLessonToServerOperationMessageColor = '';
    this.toggleDialog();
    this.toggleCloseModal();
  }

  openDeleteDialog() {
    console.log('Values for the delete dialog', this.originalCourseLesson);
    this.courseLessonDeleteDialogValues = {
      courseLessonId: this.courseLessonId,
      courseId: this.courseId,
      lessonTitle: this.originalCourseLesson?.lessonTitle ?? '',
      lessonTag: this.originalCourseLesson?.lessonTag ?? '',
      lessonSequenceOrder: this.originalCourseLesson?.lessonSequenceOrder ?? 0,
      courseLessonSupplementaryMaterialCount:
        this.courseLessonSupplementaryMaterials.length,
      courseLessonSupplementaryMaterialTotalSize: 0,
      createdAt: this.originalCourseLesson?.createdAt as string,
    };

    console.log(
      'Assigned values for the delete dialog',
      this.courseLessonDeleteDialogValues
    );

    console.log(
      'Delete dialog created at',
      this.originalCourseLesson?.createdAt
    );
    this.showDeleteDialog = !this.showDeleteDialog;
  }

  closeDeleteDialog() {
    this.showDeleteDialog = false;
  }

  closeDeleteDialogAndCloseModalWithCourseLessonListRefresh() {
    this.showDeleteDialog = false;
    this.toggleCloseModal();
    this.closeDeleteDialogAndModalAndRefreshCourseList.emit();
  }

  toggleFilePreview() {
    console.log('File preview toggled');
    if (!this.fileToUploadUrl) {
      this.fileToUploadUrl = '';
      this.showFilePreview = false;
      return;
    }
    this.showFilePreview = !this.showFilePreview;
    if (this.showFilePreview) {
      console.log('File preview shown with file url', this.fileToUploadUrl);
    }
  }
}
