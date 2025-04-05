import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetCourseLessonResourceByIdResponse } from '../../../../models/course/course-tutor-controller/get-course-lesson-resource-by-id-response';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AllowedFileTypes } from '../../../../constants/allowed-file-types.constant';
import { MaxFileTypesSizes } from '../../../../constants/max-file-types-size.constant';
import formatFileSize from '../../../../helpers/format-file-size.helper';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { UploadCourseLessonResourceRequest } from '../../../../models/course/course-tutor-controller/upload-course-lesson-resource-request';
import { ImageCompressionService } from '../../../../services/image-compression.service';

@Component({
  selector: 'app-course-tutor-lesson-resource-details',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CustomHeaderNgContentDialogBoxComponent,
  ],
  templateUrl: './course-tutor-lesson-resource-details.component.html',
  styleUrl: './course-tutor-lesson-resource-details.component.css',
})
export class CourseTutorLessonResourceDetailsComponent implements OnInit {
  courseId: string = '';
  courseLessonId: string = '';
  resourceId: string = '';

  existingResource: GetCourseLessonResourceByIdResponse | null = null;

  isEditMode: boolean = false;
  isDragging: boolean = false;

  selectedFile: File | null = null;

  urlToResource: string | null = null;

  uploadInProgress: boolean = false;
  uploadProgress: number = 0;
  uploadStatusMessage: string = '';

  downloadInProgress: boolean = false;
  downloadProgress: number = 0;
  downloadStatusMessage: string = '';

  fileErrorMessage: string = '';

  showUploadButton: boolean = false;

  showSaveDialog: boolean = false;
  saveDialogMessage: string = '';

  showDeleteDialog: boolean = false;
  deleteDialogMessage: string = '';

  maxSize: number = 0;
  titleErrorMessage: string = '';
  descriptionErrorMessage: string = '';
  urlErrorMessage: string = '';
  resourceFormGroup: FormGroup = new FormGroup({
    title: new FormControl<string>('', {
      validators: [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(100),
      ],
    }),
    description: new FormControl<string>('', {
      validators: [
        Validators.required,
        Validators.minLength(25),
        Validators.maxLength(255),
      ],
    }),
    resourceType: new FormControl<'url' | 'file'>('file'),
    file: new FormControl<File | null>(null as File | null),
    url: new FormControl<string>('', {
      validators: [
        Validators.pattern(
          /[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/
        ),
      ],
    }),
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private snackboxService: SnackboxService,
    private courseTutorControllerService: CourseTutorControllerService,
    private imageCompressionService: ImageCompressionService
  ) {
    this.courseId = this.route.snapshot.params['courseId'] as string;
    this.courseLessonId = this.route.snapshot.params['lessonId'] as string;
    this.resourceId = this.route.snapshot.params['resourceId'] as string;

    if (this.resourceId) {
      this.loadResource();
    }
  }
  ngOnInit(): void {}

  goBack() {
    this.router.navigate([
      `/tutor/course/lessons/resources/${this.courseId}/${this.courseLessonId}`,
    ]);
  }

  loadResource() {
    this.courseTutorControllerService
      .getCourseLessonResourceById(this.resourceId)
      .subscribe({
        next: (response) => {
          console.log('Response', response);
          this.existingResource = response.data;
          this.isEditMode = true;
          this.resourceFormGroup.patchValue({
            title: this.existingResource?.title,
            description: this.existingResource?.description,
            resourceType: this.existingResource?.resourceUrl ? 'url' : 'file',
            url: this.existingResource?.resourceUrl,
          });
        },
        error: (error) => {
          console.log('Error', error);
          this.snackboxService.showSnackbox(
            `Failed to load resource${
              error.error.message ? `, ${error.error.message}` : ''
            }`
          );
        },
      });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];

    if (file) {
      this.selectedFile = file;
      this.fileValidator();
    }
  }

  fileValidator() {
    const file = this.selectedFile;

    if (file) {
      const fileSize = file.size;
      const fileType = file.type;

      var maxSize: number | null = null;

      if (AllowedFileTypes.Images.includes(fileType)) {
        maxSize = MaxFileTypesSizes.MaxImageSizeInBytes;
      } else if (AllowedFileTypes.Documents.includes(fileType)) {
        maxSize = MaxFileTypesSizes.MaxDocumentSizeInBytes;
      } else if (AllowedFileTypes.Videos.includes(fileType)) {
        maxSize = MaxFileTypesSizes.MaxVideoSizeInBytes;
      } else if (AllowedFileTypes.Audio.includes(fileType)) {
        maxSize = MaxFileTypesSizes.MaxAudioSizeInBytes;
      } else {
        this.fileErrorMessage = `${this.selectedFile?.type} is not allowed!`;
        this.resourceFormGroup.controls['file'].setErrors({
          invalidFileType: true,
        });
        return;
      }

      if (fileSize > maxSize) {
        var maxSizeType: string = '';
        if (AllowedFileTypes.Images.includes(fileType)) {
          maxSizeType = 'image';
        } else if (AllowedFileTypes.Documents.includes(fileType)) {
          maxSizeType = 'document';
        } else if (AllowedFileTypes.Videos.includes(fileType)) {
          maxSizeType = 'video';
        } else if (AllowedFileTypes.Audio.includes(fileType)) {
          maxSizeType = 'audio';
        }
        this.fileErrorMessage = `File size exceeds the maximum limit of ${
          maxSize / (1024 * 1024)
        } MB for ${maxSizeType} files.`;
        this.resourceFormGroup.controls['file'].setErrors({
          invalidFileSize: true,
        });
        return;
      }
    }
    this.fileErrorMessage = '';
    this.resourceFormGroup.controls['file'].setErrors(null);
  }

  formatFileSizeTemplateFunction() {
    return formatFileSize(this.existingResource?.fileSize as number);
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      const file = event.dataTransfer.files[0];
      this.selectedFile = file;
      this.fileValidator();

      event.dataTransfer.clearData();
    }
  }

  onDownloadResource() {
    this.downloadInProgress = true;
    this.downloadStatusMessage = 'Preparing to download...';
    this.courseTutorControllerService
      .downloadCourseLessonResource(this.resourceId as string)
      .subscribe({
        next: (event) => {
          if (event.type === HttpEventType.DownloadProgress) {
            const progress = Math.round(
              (event.loaded / (event.total || 1)) * 100
            );
            this.downloadProgress = progress;
            this.downloadStatusMessage = 'Downloading...';
          } else if (event instanceof HttpResponse) {
            this.downloadInProgress = false;
            this.downloadStatusMessage = 'Download complete!';
            this.downloadProgress = 0;
            this.snackboxService.showSnackbox(
              'Download completed successfully',
              'success'
            );

            const blob = event.body as Blob;
            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            link.download = this.existingResource?.fileName || 'resource';
            link.click();

            window.URL.revokeObjectURL(downloadUrl);
          }
        },
        error: (error) => {
          this.downloadInProgress = false;
          this.downloadStatusMessage = 'Download failed!';
          this.downloadProgress = 0;
          this.snackboxService.showSnackbox(
            `Download failed: ${error.error.message || 'Unknown error'}`,
            'error'
          );
        },
      });
  }

  onCancelSaveDialog() {
    this.showSaveDialog = false;
  }
  onSaveResource() {
    this.saveDialogMessage = `Ar you sure you want to ${
      this.existingResource ? 'update' : 'save'
    } this resource?`;
    this.showSaveDialog = true;
  }
  async saveResource() {
    const resourceTypeToSave: 'url' | 'file' =
      this.resourceFormGroup.controls['resourceType'].value;

    if (AllowedFileTypes.Images.includes(this.selectedFile?.type as string)) {
      await this.handleImageCompression();
    }
    const request: UploadCourseLessonResourceRequest = {
      courseLessonId: this.courseLessonId,
      courseLessonResourceId: this.existingResource ? this.resourceId : null,
      title: this.resourceFormGroup.controls['title'].value,
      description: this.resourceFormGroup.controls['description'].value,
      resourceUrl:
        resourceTypeToSave === 'url'
          ? this.resourceFormGroup.controls['url'].value
          : null,
      file: resourceTypeToSave === 'file' ? this.selectedFile : null,
    };
    console.log('request', request);
    this.showSaveDialog = false;
    this.uploadStatusMessage = 'Preparing to upload...';
    this.uploadInProgress = true;
    this.uploadProgress = 0;

    this.courseTutorControllerService
      .uploadCourseLessonResource(request)
      .subscribe({
        next: (response) => {
          if (response.progress < 100) {
            this.uploadProgress = response.progress;
          }
          if (response.progress === 100) {
            this.uploadInProgress = false;
            this.uploadStatusMessage = 'Upload complete!';
            this.uploadProgress = 0;
          }

          if (response.response) {
            this.snackboxService.showSnackbox(
              'Resource uploaded successfully',
              'success'
            );
          }
        },
        error: (error) => {
          this.snackboxService.showSnackbox(
            `Failed to upload resource${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  onDeleteResource() {
    this.deleteDialogMessage = 'Are you sure you want to delete the resource?';
    this.showDeleteDialog = true;
  }
  onCancelDeleteDialog() {
    this.showDeleteDialog = false;
  }
  deleteResource() {
    this.courseTutorControllerService
      .deleteCourseLessonResourceById(this.resourceId as string)
      .subscribe({
        next: (response) => {
          this.snackboxService.showSnackbox(
            'Resource deleted successfully',
            'success'
          );
          this.router.navigate([
            '/tutor/course/lesson/resources',
            this.courseId,
            this.courseLessonId,
          ]);
        },
        error: (error) => {
          this.snackboxService.showSnackbox(
            `Failed to delete resource: ${error.error.message}`,
            'error'
          );
        },
      });
    this.showDeleteDialog = false;
  }

  private async handleImageCompression(): Promise<void> {
    try {
      const compressedFile = await this.imageCompressionService.compressImage(
        this.selectedFile as File,
        {
          maxWidth: 1024,
          maxHeight: 1024,
          quality: 0.5,
        }
      );

      console.log(
        `Original file size: ${this.selectedFile?.size} - Compressed file size: ${compressedFile.size}`
      );

      this.selectedFile = compressedFile;
    } catch (error) {
      console.log('Compression failed: ', error);
      this.snackboxService.showSnackbox(
        'We failed to compress the thumbnail image, ' + error,
        'error'
      );
    }
  }
}
