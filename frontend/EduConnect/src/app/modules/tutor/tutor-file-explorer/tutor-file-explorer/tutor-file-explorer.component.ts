import { Component, OnInit, NgModule } from '@angular/core';
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
import { FormsModule, NgModel, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-tutor-file-explorer',
  standalone: true,
  imports: [
    CommonModule,
    FileSizePipe,
    CustomHeaderNgContentDialogBoxComponent,
    FormsModule,
  ],
  templateUrl: './tutor-file-explorer.component.html',
  styleUrl: './tutor-file-explorer.component.css',
})
export class TutorFileExplorerComponent implements OnInit {
  allFilesUploadedByPerson: GetAllFilesUploadedByPersonResponse[] = [];
  showDeleteDialog: boolean = false;
  deleteDialogMessage: string = '';
  fileToDelete: GetAllFilesUploadedByPersonResponse | null = null;

  originalFile: GetAllFilesUploadedByPersonResponse | null = null;
  selectedFileForEdit: GetAllFilesUploadedByPersonResponse | null = null;
  editTitle: string = '';
  editDescription: string = '';
  editFileName: string = '';

  sortColumn: string | null = null;
  sortDirection: 'asc' | 'desc' = 'asc';

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

  startEdit(file: GetAllFilesUploadedByPersonResponse) {
    this.originalFile = file;
    this.selectedFileForEdit = file;
  }

  saveEdit() {
    if (!this.selectedFileForEdit) return;

    this.selectedFileForEdit.title =
      this.selectedFileForEdit.title?.trim() || '';
    this.selectedFileForEdit.description =
      this.selectedFileForEdit.description?.trim() || '';

    // Validation checks
    if (this.selectedFileForEdit.title.length === 0) {
      this.snackboxService.showSnackbox('Title is required', 'error');
      return;
    }

    if (this.selectedFileForEdit.title.length > 100) {
      this.snackboxService.showSnackbox(
        'Title cannot exceed 100 characters',
        'error'
      );
      return;
    }

    if (this.selectedFileForEdit.description.length === 0) {
      this.snackboxService.showSnackbox('Description is required', 'error');
      return;
    }

    if (this.selectedFileForEdit.description.length < 25) {
      this.snackboxService.showSnackbox(
        'Description must be at least 25 characters',
        'error'
      );
      return;
    }

    if (this.selectedFileForEdit.description.length > 255) {
      this.snackboxService.showSnackbox(
        'Description cannot exceed 255 characters',
        'error'
      );
      return;
    }

    console.log('Finished editing, saving,', this.selectedFileForEdit);
    const index = this.allFilesUploadedByPerson.findIndex(
      (file) => file.id === this.selectedFileForEdit?.id
    );

    if (index !== -1) {
      this.allFilesUploadedByPerson[index] = { ...this.selectedFileForEdit };
      this.snackboxService.showSnackbox(
        'File details updated successfully',
        'success'
      );
    } else {
      this.snackboxService.showSnackbox(
        'Error: File not found in the list',
        'error'
      );
    }

    if (
      this.selectedFileForEdit.fileSourceType ===
      FileSourceType.CourseTeachingResource
    ) {
      this.courseTutorControllerService
        .updateCourseTeachingResourceMetadata({
          courseTeachingResourceId: this.selectedFileForEdit.id as string,
          title: this.selectedFileForEdit.title,
          description: this.selectedFileForEdit.description,
        })
        .subscribe({
          next: (response) => {
            console.log('Response:', response);
          },
          error: (error) => {
            console.log('Error:', error);
            this.snackboxService.showSnackbox(
              `Error updating file details: ${
                error.error.message || 'Unknown error'
              }`,
              'error'
            );
            this.getAllFilesUploadedByPerson();
          },
        });
    }
    if (
      this.selectedFileForEdit.fileSourceType ===
      FileSourceType.CourseLessonResource
    ) {
      this.courseTutorControllerService
        .updateCourseLessonResourceMetadata({
          courseLessonResourceId: this.selectedFileForEdit.id as string,
          title: this.selectedFileForEdit.title,
          description: this.selectedFileForEdit.description,
        })
        .subscribe({
          next: (response) => {
            console.log(response);
          },
          error: (error) => {
            console.log('Error:', error);
            this.snackboxService.showSnackbox(
              `Error updating file details: ${
                error.error.message || 'Unknown error'
              }`,
              'error'
            );
          },
        });
    }

    this.selectedFileForEdit = null;
    this.originalFile = null;
  }

  cancelEdit() {
    this.selectedFileForEdit = null;
    this.originalFile = null;
  }

  get showAutoGeneratedNote(): boolean {
    return this.allFilesUploadedByPerson.some((file) => !this.isEditable(file));
  }

  isEditable(file: GetAllFilesUploadedByPersonResponse): boolean {
    return (
      file.fileSourceType === FileSourceType.CourseTeachingResource ||
      file.fileSourceType === FileSourceType.CourseLessonResource
    );
  }

  sort(column: string) {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    console.log('Sort by column:', column, 'Direction:', this.sortDirection);

    if (this.sortColumn === 'size') {
      this.allFilesUploadedByPerson.sort((a, b) => {
        const sizeA = a.fileSize;
        const sizeB = b.fileSize;
        return this.sortDirection === 'asc'
          ? (sizeA || 0) - (sizeB || 0)
          : (sizeB || 0) - (sizeA || 0);
      });
    }

    if (this.sortColumn === 'type') {
      this.allFilesUploadedByPerson.sort((a, b) => {
        const typeA = a.contentType;
        const typeB = b.contentType;
        return this.sortDirection === 'asc'
          ? typeA.localeCompare(typeB)
          : typeB.localeCompare(typeA);
      });
    }

    if (this.sortColumn === 'uploadDate') {
      this.allFilesUploadedByPerson.sort((a, b) => {
        const dateA = new Date(a.createdAt).getTime();
        const dateB = new Date(b.createdAt).getTime();
        return this.sortDirection === 'asc' ? dateA - dateB : dateB - dateA;
      });
    }

    if (this.sortColumn === 'lastUpdated') {
      this.allFilesUploadedByPerson.sort((a, b) => {
        const dateA = a.updatedAt ? new Date(a.updatedAt).getTime() : 0;
        const dateB = b.updatedAt ? new Date(b.updatedAt).getTime() : 0;
        return this.sortDirection === 'asc' ? dateA - dateB : dateB - dateA;
      });
    }
  }
}
