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
import { TextInputComponentComponent } from '../../../../common/input/text/text-input-component/text-input-component.component';
import { Form } from '@angular/forms';
import { TextAreaInputComponentComponent } from '../../../../common/input/text/text-area-input-component/text-area-input-component.component';
import { TextAreaResizeType } from '../../../../../enums/textarea-resize-types.enum';
import { Editor, NgxEditorModule } from 'ngx-editor';
import { CourseLessonSupplementaryMaterial } from '../../../../_models/course/course-lesson-supplementary-material/course-lesson-supplementary-material.model';
import { DomSanitizer } from '@angular/platform-browser';
import { NgFor, NgIf } from '@angular/common';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import { distinctUntilChanged } from 'rxjs';
import { CourseCreateService } from '../../../../services/course/course-create-service.service';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FloatingWarningBoxComponent } from '../../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';

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
  ],
  templateUrl: './course-lesson-modal.component.html',
  styleUrl: './course-lesson-modal.component.css',
})
export class CourseLessonModalComponent implements OnInit {
  @Input() modalTitle: string = 'Modal Title';
  @Input() isCreateOrEditMode: boolean = true;
  @Input() courseId: string = 'e6c87eb6-f462-4516-8d17-ddbd9b3b1def';
  @Input() courseLessonId: string = '';

  @Output() closeModal: EventEmitter<void> = new EventEmitter<void>();

  courseCreateService: CourseCreateService = inject(CourseCreateService);

  showCloseButton: boolean = true;

  sanitizer: DomSanitizer = inject(DomSanitizer);

  //Add Editor configuration
  public editor: Editor = new Editor();

  showSupplementaryMaterialUpload: boolean = false;
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
    this.loadCourseLessonSupplementaryMaterials();
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
    }
    if (
      this.courseLessonModalFormGroup.valid ||
      this.lessonContentFormGroup.valid
    ) {
      this.submitDataMessage = 'All fields are valid!';
      this.submitDataMessageColor = 'green';
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
      }
      if (
        this.courseLessonModalFormGroup.valid ||
        this.lessonContentFormGroup.valid
      ) {
        this.submitDataMessage = 'All fields are valid!';
        this.submitDataMessageColor = 'green';
      }
    });
    this.lessonContentFormGroup.valueChanges.subscribe(() => {
      if (
        this.courseLessonModalFormGroup.invalid ||
        this.lessonContentFormGroup.invalid
      ) {
        this.submitDataMessage = 'Please remove any errors to proceed!';
        this.submitDataMessageColor = 'red';
      }
      if (
        this.courseLessonModalFormGroup.valid ||
        this.lessonContentFormGroup.valid
      ) {
        this.submitDataMessage = 'All fields are valid!';
        this.submitDataMessageColor = 'green';
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
      //Call the function to create the course lesson
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
    }
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
}
