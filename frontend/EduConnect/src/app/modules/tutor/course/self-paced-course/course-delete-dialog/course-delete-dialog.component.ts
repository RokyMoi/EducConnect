import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SubmitButtonComponent } from '../../../../../common/button/submit-button/submit-button.component';
import { CourseCreateService } from '../../../../../services/course/course-create-service.service';
import { NgIf } from '@angular/common';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';

@Component({
  standalone: true,
  selector: 'app-course-delete-dialog',
  imports: [SubmitButtonComponent, NgIf, MatProgressSpinner],
  templateUrl: './course-delete-dialog.component.html',
  styleUrl: './course-delete-dialog.component.css',
})
export class CourseDeleteDialogComponent {
  @Input() courseLessonId: string = '';
  @Input() lessonTitle: string = '';
  @Input() lessonTag: string = '';
  @Input() lessonPosition: number = 0;
  @Input() createdAt: string = '';
  @Input() numberOfMaterials: number = 0;
  @Input() courseCreateService!: CourseCreateService;
  @Input() isAccessedFromCourseLessonModal: boolean = false;

  @Output() closeDeleteDialog: EventEmitter<void> = new EventEmitter<void>();
  @Output() closeDeleteDialogAndRefreshCourseLessonList: EventEmitter<void> =
    new EventEmitter<void>();

  //Text to display once the data has been retrieved from the server
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';
  //Variables for styling the confirm operation button
  confirmDeleteButtonText: string = 'Yes, delete this lesson';
  confirmDeleteButtonColor: string = 'red';

  //Variables for styling the cancel operation button
  cancelDeleteButtonText: string = "No, don't delete this lesson";
  cancelDeleteButtonColor: string = 'green';

  //Variables for styling the button for closing the dialog
  closeButtonText: string = 'Ok, close';
  closeButtonColor: string = 'blue';

  //Variables for handling the delete operation
  isDataTransmissionActive: boolean = false;
  isDataTransmissionComplete: boolean = false;
  isDataDeletionSuccessful: boolean | null = null;
  operationStatus: string = '';
  operationStatusColor: string = '';

  confirmDelete() {
    this.isDataTransmissionActive = true;
    this.isDataTransmissionComplete = false;
    this.operationStatus = 'Deleting lesson...';
    this.operationStatusColor = 'red';
    this.courseCreateService
      .deleteCourseLessonAndAssociatedDataByCourseLessonId(this.courseLessonId)
      .subscribe((response) => {
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;
        if (response.success === 'true') {
          this.operationStatus = 'Lesson deleted successfully';
          this.operationStatusColor = 'green';
          this.isDataDeletionSuccessful = true;
        } else {
          this.operationStatus = 'Error deleting lesson, ' + response.message;
          this.operationStatusColor = 'red';
          this.isDataDeletionSuccessful = false;
        }
      });
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('ba', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }

  closeDialog() {
    console.log('Close dialog triggered');
    this.closeDeleteDialog.emit();
  }

  closeDialogAndRefreshCourseLessonList() {
    console.log('Close dialog and refresh course lesson list triggered');
    this.closeDeleteDialogAndRefreshCourseLessonList.emit();
  }
}
