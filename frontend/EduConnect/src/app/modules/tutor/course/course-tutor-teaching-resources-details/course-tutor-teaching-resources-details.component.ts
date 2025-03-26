import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { AllowedFileTypes } from '../../../../constants/allowed-file-types.constant';
import { MaxFileTypesSizes } from '../../../../constants/max-file-types-size.constant';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { UploadCourseTeachingResourceRequest } from '../../../../models/course/course-tutor-controller/upload-course-teaching-resource-request';
import { GetCourseTeachingResourceResponse } from '../../../../models/course/course-tutor-controller/get-course-teaching-resource-response';
import formatFileSize from '../../../../helpers/format-file-size.helper';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { ImageCompressionService } from '../../../../services/image-compression.service';
import { VideoCompressionService } from '../../../../services/video-compression.service';
@Component({
  selector: 'app-course-tutor-teaching-resources-details',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    CustomHeaderNgContentDialogBoxComponent,
  ],
  templateUrl: './course-tutor-teaching-resources-details.component.html',
  styleUrl: './course-tutor-teaching-resources-details.component.css',
})
export class CourseTutorTeachingResourcesDetailsComponent {
  courseId: string | null = null;
  resourceId: string | null = null;

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

  existingResource: GetCourseTeachingResourceResponse | null = null;

  maxSize: number = 0;
  titleErrorMessage: string = '';
  descriptionErrorMessage: string = '';
  urlErrorMessage: string = '';
  resourceFormGroup: FormGroup = new FormGroup({
    title: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(100),
      ],
    }),
    description: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(25),
        Validators.maxLength(255),
      ],
    }),
    resourceType: new FormControl<'file' | 'url'>('file'),
    file: new FormControl(null as File | null),
    url: new FormControl('', {
      validators: [
        Validators.pattern(
          /[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/
        ),
      ],
    }),
  });
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService,
    private imageCompressionService: ImageCompressionService,
    private videoCompressionService: VideoCompressionService,
    private courseTutorControllerService: CourseTutorControllerService
  ) {
    this.route.paramMap.subscribe((params) => {
      this.courseId = params.get('courseId') as string;
      this.resourceId = (params.get('resourceId') as string) || null;

      this.isEditMode = !!this.resourceId;

      if (this.isEditMode) {
        console.log('Editing existing resource:', this.resourceId);
        this.loadResource();
      } else {
        console.log('Creating new resource for course:', this.courseId);
      }
    });
  }
  goBack() {
    this.router.navigate(['/tutor/course/teaching-resources/' + this.courseId]);
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

  onSaveResource() {
    const resourceTypeToSave: 'url' | 'file' =
      this.resourceFormGroup.controls['resourceType'].value;

    this.saveDialogMessage = `Are you sure you want to save this ${resourceTypeToSave}?`;
    this.showSaveDialog = true;
  }

  onCancelSaveDialog() {
    this.showSaveDialog = false;
  }
  async saveResource() {
    const resourceTypeToSave: 'url' | 'file' =
      this.resourceFormGroup.controls['resourceType'].value;

    if (AllowedFileTypes.Images.includes(this.selectedFile?.type as string)) {
      await this.handleImageCompression();
    }
    //  else if (
    //   AllowedFileTypes.Videos.includes(this.selectedFile?.type as string)
    // ) {
    //   console.log('Compressing video...');
    //   console.log('Video size before compression:', this.selectedFile?.size);
    //   this.showSaveDialog = false;
    //   this.snackboxService.showSnackbox(
    //     'Compression in progress, please wait...',
    //     'info'
    //   );
    //   // await this.videoCompressionService
    //   //   .compressVideo(this.selectedFile as File)
    //   //   .then((compressedFile) => {
    //   //     this.selectedFile = compressedFile;
    //   //   });
    //   console.log('Video size after compression:', this.selectedFile?.size);
    // }

    const request: UploadCourseTeachingResourceRequest = {
      courseId: this.courseId,
      courseTeachingResourceId: this.resourceId ? this.resourceId : null,
      title: this.resourceFormGroup.controls['title'].value as string,
      description: this.resourceFormGroup.controls['description']
        .value as string,
      resourceUrl:
        resourceTypeToSave === 'url'
          ? (this.resourceFormGroup.controls['url'].value as string)
          : null,
      resourceFile: resourceTypeToSave === 'file' ? this.selectedFile : null,
    };

    console.log('request', request);
    this.showSaveDialog = false;
    this.uploadStatusMessage = 'Preparing to upload...';
    this.uploadInProgress = true;
    this.uploadProgress = 0;

    this.courseTutorControllerService
      .uploadCourseTeachingResource(request)
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

  loadResource() {
    this.courseTutorControllerService
      .getCourseTeachingResource(this.resourceId as string)
      .subscribe({
        next: (response) => {
          console.log('response', response);
          this.existingResource = response.data;
          if (this.existingResource) {
            this.resourceFormGroup.controls['title'].setValue(
              this.existingResource.title as string
            );
            this.resourceFormGroup.controls['description'].setValue(
              this.existingResource.description as string
            );
            if (this.existingResource.resourceUrl) {
              this.resourceFormGroup.controls['resourceType'].setValue('url');
              this.resourceFormGroup.controls['url'].setValue(
                this.existingResource.resourceUrl as string
              );
            }
          }
        },
        error: (error) => {
          console.log('error', error);
          this.snackboxService.showSnackbox(
            `Failed to load resource${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  formatFileSizeTemplateFunction() {
    return formatFileSize(this.existingResource?.fileSize as number);
  }

  onDownloadResource() {
    this.downloadInProgress = true;
    this.downloadStatusMessage = 'Preparing to download...';
    this.courseTutorControllerService
      .downloadCourseTeachingResource(this.resourceId as string)
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

  onDeleteResource() {
    this.deleteDialogMessage = 'Are you sure you want to delete this resource?';
    this.showDeleteDialog = true;
  }

  onCancelDeleteDialog() {
    this.showDeleteDialog = false;
  }

  deleteResource() {
    this.courseTutorControllerService
      .deleteCourseTeachingResource(this.resourceId as string)
      .subscribe({
        next: (response) => {
          this.snackboxService.showSnackbox(
            'Resource deleted successfully',
            'success'
          );
          this.router.navigate([
            '/tutor/course/teaching-resources/' + this.courseId,
          ]);
        },
        error: (error) => {
          this.snackboxService.showSnackbox(
            `Failed to delete resource${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
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
}
