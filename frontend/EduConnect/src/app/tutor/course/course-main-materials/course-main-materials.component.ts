import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import { NgFor, NgIf } from '@angular/common';
import { CourseCreateService } from '../../../services/course/course-create-service.service';
import { ReferenceService } from '../../../services/reference/reference.service';
import { CourseMainMaterial } from '../../../_models/course/course-main-material/course-main-material.course.model';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import DateHelper from '../../../helpers/date.helper';
import { FloatingWarningBoxComponent } from '../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';

@Component({
  standalone: true,
  selector: 'app-course-main-materials',
  imports: [
    SubmitButtonComponent,
    NgIf,
    NgFor,
    ReactiveFormsModule,
    FloatingWarningBoxComponent,
  ],
  templateUrl: './course-main-materials.component.html',
  styleUrl: './course-main-materials.component.css',
})
export class CourseMainMaterialsComponent implements OnInit {
  @Input() referenceService!: ReferenceService;
  @Input() courseCreateService!: CourseCreateService;
  @Input() courseId!: string;

  //Output variable used to communicate to the parent to switch to the next step
  @Output() goToNextStep: EventEmitter<void> = new EventEmitter<void>();

  //Output variable used to communicate to the parent that this step has been completed
  @Output() courseMainMaterialsStepCompleted: EventEmitter<boolean> =
    new EventEmitter<boolean>();

  @ViewChild('fileInput') fileInput!: ElementRef;

  currentNumberOfFiles: number = 0;
  currentSpaceTakenUpInMb: number = 0;

  uploadFileButtonText: string = 'Upload selected file';
  uploadFileButtonColor: string = 'green';

  fileUploadInstructionText: string = 'Select a file to upload';

  fileUploadWarningText: string = '';

  selectedFile: File | null = null;

  fileToUploadName: string = '';
  fileToUploadSizeInMegabytes: number = 0;
  fileToUploadType: string = '';
  fileToUploadFileCategory: string = '';

  fileUploadResultMessage: string = '';
  fileUploadResultColor: string = 'green';

  isDataTransmissionActive: boolean = false;
  isDataTransmissionComplete: boolean = false;

  courseMainMaterialsArray: CourseMainMaterial[] = [];

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

  //Variables for the floating warning box that shows up when the user tries to delete the selected file
  floatingWarningDeleteBoxTitle: string = '';
  floatingWarningDeleteBoxMessage: string = '';
  floatingWarningDeleteBoxMessageColor: string = '';
  floatingWarningDeleteBoxShow: boolean = false;
  floatingWarningDeleteBoxFileToDeleteName: string = '';
  floatingWarningDeleteBoxFileToDeleteIndex: string = '';

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

  //Variables for the button that shows up when the communication with the server to delete the selected file is completed no matter if it was successful or not
  //This button closes the floating warning box
  closeFloatingBoxButtonText: string = 'Ok, close';
  closeFloatingBoxButtonColor: string = 'blue';

  ngOnInit(): void {
    this.loadCourseMainMaterials();
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    this.fileUploadResultMessage = '';
    console.log('Selected file:', this.selectedFile);
    if (this.selectedFile) {
      //Check if the file type is allowed
      if (!this.validateFileType(this.selectedFile)) {
        console.log('File type not allowed');
        this.fileUploadWarningText =
          'This file type is not allowed, choose another file';
        this.fileUploadInstructionText = '';
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

      this.courseCreateService
        .uploadFileAsCourseMainMaterial(
          this.courseId,
          givenFileName,
          givenDateTimeOfCreation,
          this.selectedFile
        )
        .subscribe((response) => {
          this.isDataTransmissionActive = false;
          this.isDataTransmissionComplete = true;
          if (response.success === 'true') {
            this.fileUploadResultMessage = 'File uploaded successfully';
            this.fileUploadResultColor = 'green';

            //Clear the file input element
            this.fileInput.nativeElement.value = '';
          }
          if (response.success === 'false') {
            this.fileUploadResultMessage =
              'Failed to upload file, ' + response.message;
            this.fileUploadResultColor = 'red';
          }
          this.selectedFile = null;
          this.loadCourseMainMaterials();
        });
    }
  }

  loadCourseMainMaterials() {
    this.courseCreateService
      .getCourseMainMaterials(this.courseId)
      .subscribe((response) => {
        this.courseMainMaterialsArray = [];
        this.currentNumberOfFiles = 0;
        this.currentSpaceTakenUpInMb = 0;
        if (response.success === 'true') {
          this.courseMainMaterialsArray = response.data.courseMainMaterials;
          this.currentNumberOfFiles = response.data.courseMainMaterials.length;
          this.currentSpaceTakenUpInMb =
            Math.round(
              (response.data.courseMainMaterialsTotalSize / (1024 * 1024)) * 1e2
            ) / 1e2;
          response.data.courseMainMaterialsTotalSize;
          console.log('Course main materials', this.courseMainMaterialsArray);
        }
      });
  }

  getFileSizeInMb(fileSizeInBytes: string): number {
    return (
      Math.round((Number.parseInt(fileSizeInBytes) / (1024 * 1024)) * 1e2) / 1e2
    );
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

  deleteCourseMainMaterial(courseMainMaterialId: string, index: number) {
    console.log(
      'Deleting course main material with ID: ',
      courseMainMaterialId
    );

    this.floatingWarningDeleteBoxTitle = 'Delete file';
    this.floatingWarningDeleteBoxMessage =
      'Are you sure you want to delete this file?';
    this.floatingWarningDeleteBoxMessageColor = 'red';
    this.floatingWarningDeleteBoxFileToDeleteIndex = `File position in the list: ${
      index + 1
    }`;
    this.floatingWarningDeleteBoxFileToDeleteName = `File name: ${this.courseMainMaterialsArray[index].fileName}`;
    this.selectedFileCourseMainMaterialId = courseMainMaterialId;
    this.floatingWarningDeleteBoxConfirmButtonText = 'Yes, delete this file!';
    this.floatingWarningDeleteBoxConfirmButtonColor = 'red';

    this.floatingWarningDeleteBoxCancelButtonText =
      "No, don't delete this file!";
    this.floatingWarningDeleteBoxCancelButtonColor = 'blue';
    this.floatingWarningDeleteBoxShow = true;
  }

  confirmDelete() {
    if (this.selectedFileCourseMainMaterialId) {
      this.courseCreateService
        .deleteCourseMainMaterialByCourseMainMaterialId(
          this.selectedFileCourseMainMaterialId
        )
        .subscribe((response) => {
          console.log('Response:', response);
          this.isDeleteOperationComplete = true;
          this.loadCourseMainMaterials();
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
  cancelDelete() {
    this.floatingWarningDeleteBoxTitle = '';
    this.floatingWarningDeleteBoxMessageColor = '';
    this.floatingWarningDeleteBoxMessage = '';
    this.floatingWarningDeleteBoxFileToDeleteIndex = '';
    this.floatingWarningDeleteBoxFileToDeleteName = '';
    this.floatingWarningDeleteBoxConfirmButtonColor = '';
    this.floatingWarningDeleteBoxConfirmButtonText = '';
    this.floatingWarningDeleteBoxCancelButtonColor = '';
    this.floatingWarningDeleteBoxCancelButtonText = '';
    this.isDeleteOperationComplete = false;
    this.floatingWarningDeleteBoxShow = false;
  }

  downloadCourseMainMaterial(courseMainMaterialId: string) {
    this.courseCreateService
      .downloadCourseMainMaterialByCourseMainMaterialId(courseMainMaterialId)
      .subscribe((response) => {
        if (response.isFile) {
          // If it's a file, trigger the download.
          const blob = response.file;
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;

          // Extract the filename from the Content-Disposition header.
          const contentDisposition = blob.headers?.get('Content-Disposition');
          const filename = contentDisposition
            ? contentDisposition.split('filename=')[1].replace(/['"]/g, '')
            : 'downloaded_file';

          a.download = filename;
          a.click();
          window.URL.revokeObjectURL(url);
        } else {
          // If it's an error, display the message.
          const { message } = response;
          alert(`Error: ${message}`);
        }
      });
  }
}
