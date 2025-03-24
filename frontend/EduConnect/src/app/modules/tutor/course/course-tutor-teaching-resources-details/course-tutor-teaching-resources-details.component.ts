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
  uploadProgress: number = 50;
  uploadStatusMessage: string = '';

  fileErrorMessage: string = '';

  showUploadButton: boolean = false;

  showSaveDialog: boolean = false;
  saveDialogMessage: string = '';

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

  saveResource() {
    const resourceTypeToSave: 'url' | 'file' =
      this.resourceFormGroup.controls['resourceType'].value;

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
}
