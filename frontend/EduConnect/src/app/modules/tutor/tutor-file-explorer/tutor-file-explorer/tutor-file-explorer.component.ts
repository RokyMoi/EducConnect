import { Component, OnInit } from '@angular/core';
import { PersonFilesControllerService } from '../../../../services/shared/person-files-controller.service';
import { GetAllFilesUploadedByPersonResponse } from '../../../../models/shared/person-files-controller/get-all-files-uploaded-by-person-response';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FileSizePipe } from '../../../../pipes/file-size.pipe';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { FileSourceType } from '../../../../../enums/file-source-type.enum';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';

@Component({
  selector: 'app-tutor-file-explorer',
  standalone: true,
  imports: [
    CommonModule,
    FileSizePipe,
    CustomHeaderNgContentDialogBoxComponent,
  ],
  templateUrl: './tutor-file-explorer.component.html',
  styleUrl: './tutor-file-explorer.component.css',
})
export class TutorFileExplorerComponent implements OnInit {
  allFilesUploadedByPerson: GetAllFilesUploadedByPersonResponse[] = [];
  showDeleteDialog: boolean = false;
  deleteDialogMessage: string = '';
  fileToDelete: GetAllFilesUploadedByPersonResponse | null = null;

  constructor(
    private router: Router,
    private personFilesControllerService: PersonFilesControllerService,
    private courseTutorControllerService: CourseTutorControllerService,
    private snackboxService: SnackboxService
  ) {}

  ngOnInit(): void {
    this.getAllFilesUploadedByPerson();
  }

  goBack() {
    this.router.navigate(['/tutor/dashboard']);
  }
  getAllFilesUploadedByPerson() {
    this.personFilesControllerService.getAllFilesUploadedByPerson().subscribe({
      next: (response) => {
        console.log('Response:', response);
        this.allFilesUploadedByPerson = response.data;
      },
      error: (error) => {
        console.log('Error:', error);
      },
    });
  }

  onOpenFile(source: string) {
    this.router.navigate([source]);
  }

  onDownloadFile(file: GetAllFilesUploadedByPersonResponse) {
    if (
      file.fileSourceType === FileSourceType.CoursePromotionImage ||
      file.fileSourceType === FileSourceType.CourseThumbnail
    ) {
      console.log('File type cannot be downloaded');
      this.snackboxService.showSnackbox(
        `${
          file.fileSourceType === FileSourceType.CoursePromotionImage
            ? 'Course Promotion Image'
            : 'Course Thumbnail'
        } cannot be downloaded`,
        'error'
      );
    }

    if (file.fileSourceType === FileSourceType.CourseTeachingResource) {
      this.courseTutorControllerService
        .downloadCourseTeachingResource(file.id)
        .subscribe({
          next: (event) => {
            if (event.type === HttpEventType.DownloadProgress) {
              const progress = Math.round(
                (event.loaded / (event.total || 1)) * 100
              );
              console.log('Download progress:', progress);
            } else if (event instanceof HttpResponse) {
              this.snackboxService.showSnackbox(
                'Download completed successfully',
                'success'
              );

              const blob = event.body as Blob;
              const downloadUrl = window.URL.createObjectURL(blob);
              const link = document.createElement('a');
              link.href = downloadUrl;
              link.download = file.fileName || 'resource';
              link.click();

              window.URL.revokeObjectURL(downloadUrl);
            }
          },
          error: (error) => {
            this.snackboxService.showSnackbox(
              `Download failed: ${error.error.message || 'Unknown error'}`,
              'error'
            );
          },
        });
    }

    if (file.fileSourceType === FileSourceType.CourseLessonResource) {
      this.courseTutorControllerService
        .downloadCourseLessonResource(file.id)
        .subscribe({
          next: (event) => {
            if (event.type === HttpEventType.DownloadProgress) {
              const progress = Math.round(
                (event.loaded / (event.total || 1)) * 100
              );
              console.log('Download progress:', progress);
            } else if (event instanceof HttpResponse) {
              this.snackboxService.showSnackbox(
                'Download completed successfully',
                'success'
              );

              const blob = event.body as Blob;
              const downloadUrl = window.URL.createObjectURL(blob);
              const link = document.createElement('a');
              link.href = downloadUrl;
              link.download = file.fileName || 'resource';
              link.click();

              window.URL.revokeObjectURL(downloadUrl);
            }
          },
          error: (error) => {
            this.snackboxService.showSnackbox(
              `Download failed: ${error.error.message || 'Unknown error'}`,
              'error'
            );
          },
        });
    }
  }
  onCancelDeleteDialog() {
    this.showDeleteDialog = false;
  }
  onDeleteFile(file: GetAllFilesUploadedByPersonResponse) {
    this.fileToDelete = file;
    this.deleteDialogMessage = `Are you sure you want to delete ${file.fileName}?`;
    this.showDeleteDialog = true;
  }

  deleteFile() {
    if (!this.fileToDelete) return;

    if (this.fileToDelete.fileSourceType === FileSourceType.CourseThumbnail) {
      this.courseTutorControllerService
        .deleteCourseThumbnailByThumbnailId(this.fileToDelete.id as string)
        .subscribe({
          next: (response) => {
            console.log('Response:', response);
            this.snackboxService.showSnackbox(
              `${this.fileToDelete?.fileName} deleted successfully`,
              'success'
            );
            this.showDeleteDialog = false;
            this.getAllFilesUploadedByPerson();
          },
          error: (error) => {
            console.log('Error:', error);
            this.snackboxService.showSnackbox(
              `Error deleting ${this.fileToDelete?.fileName}: ${
                error.error.message || 'Unknown error'
              }`,
              'error'
            );
          },
        });
    }

    if (
      this.fileToDelete.fileSourceType === FileSourceType.CoursePromotionImage
    ) {
      this.courseTutorControllerService
        .deleteCoursePromotionImage(this.fileToDelete.id as string)
        .subscribe({
          next: (response) => {
            console.log('Response:', response);
            this.snackboxService.showSnackbox(
              `${this.fileToDelete?.fileName} deleted successfully`,
              'success'
            );
            this.showDeleteDialog = false;
            this.getAllFilesUploadedByPerson();
          },
          error: (error) => {
            console.log('Error:', error);
            this.snackboxService.showSnackbox(
              `Error deleting ${this.fileToDelete?.fileName}: ${
                error.error.message || 'Unknown error'
              }`,
              'error'
            );
          },
        });
    }

    if (
      this.fileToDelete.fileSourceType === FileSourceType.CourseTeachingResource
    ) {
      this.courseTutorControllerService
        .deleteCourseTeachingResource(this.fileToDelete.id as string)
        .subscribe({
          next: (response) => {
            console.log('Response:', response);
            this.snackboxService.showSnackbox(
              `${this.fileToDelete?.fileName} deleted successfully`,
              'success'
            );
            this.showDeleteDialog = false;
            this.getAllFilesUploadedByPerson();
          },
          error: (error) => {
            console.log('Error:', error);
            this.snackboxService.showSnackbox(
              `Error deleting ${this.fileToDelete?.fileName}: ${
                error.error.message || 'Unknown error'
              }`,
              'error'
            );
          },
        });
    }

    if (
      this.fileToDelete.fileSourceType === FileSourceType.CourseLessonResource
    ) {
      this.courseTutorControllerService
        .deleteCourseLessonResourceById(this.fileToDelete.id as string)
        .subscribe({
          next: (response) => {
            console.log('Response:', response);
            this.snackboxService.showSnackbox(
              `${this.fileToDelete?.fileName} deleted successfully`,
              'success'
            );
            this.showDeleteDialog = false;
            this.getAllFilesUploadedByPerson();
          },
          error: (error) => {
            console.log('Error:', error);
            this.snackboxService.showSnackbox(
              `Error deleting ${this.fileToDelete?.fileName}: ${
                error.error.message || 'Unknown error'
              }`,
              'error'
            );
          },
        });
    }
  }
}
