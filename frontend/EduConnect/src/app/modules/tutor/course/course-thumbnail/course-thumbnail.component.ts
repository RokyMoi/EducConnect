import { CommonModule } from '@angular/common';
import { Component, computed, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { UploadCourseThumbnailRequest } from '../../../../models/course/course-tutor-controller/upload-course-thumbnail-request';
import { ActivatedRoute, Router } from '@angular/router';
import { DefaultServerResponse } from '../../../../models/shared/default-server-response';
import { ImageCompressionService } from '../../../../services/image-compression.service';

@Component({
  selector: 'app-course-thumbnail',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    CustomHeaderNgContentDialogBoxComponent,
  ],
  templateUrl: './course-thumbnail.component.html',
  styleUrl: './course-thumbnail.component.css',
})
export class CourseThumbnailComponent implements OnInit {
  courseId: string = '';

  isNewOrExisting: boolean = true;

  selectedFile: File | null = null;
  previewUrl: string | null = null;

  acceptedFileTypes: string[] = [
    'image/jpeg',
    'image/png',
    'image/jpg',
    'image/gif',
  ];
  maxFileSize: number = 5 * 1024 * 1024; // 10MB

  thumbnailFormGroup = new FormGroup({
    useAzureStorage: new FormControl(false),
    file: new FormControl(null, {
      validators: [Validators.required, this.fileValidator.bind(this)],
    }),
  });

  fileErrorMessage: string = '';

  showSaveDialog: boolean = false;
  saveDialogMessage: string = '';

  showRemoveDialog: boolean = false;

  uploadInProgress: boolean = false;
  uploadProgress: number = 50;
  uploadStatusMessage: string = '';

  isDragging: boolean = false;

  constructor(
    private snackboxService: SnackboxService,
    private courseTutorControllerService: CourseTutorControllerService,
    private imageCompressionService: ImageCompressionService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId') as string;
  }

  ngOnInit(): void {
    this.courseTutorControllerService
      .getCourseThumbnail(this.courseId)
      .subscribe({
        next: (response) => {
          const imageURL = URL.createObjectURL(response);
          this.previewUrl = imageURL;
          this.isNewOrExisting = false;
        },
        error: (error) => {
          console.log(error);
        },
      });

    this.thumbnailFormGroup.controls.useAzureStorage.valueChanges.subscribe(
      (value) => {
        console.log('Use Azure Storage: ', value);
      }
    );
  }

  async onFileSelected(event: any) {
    this.isNewOrExisting = true;
    this.selectedFile = event.target.files[0];
    console.log('Added file: ', this.selectedFile);

    const fileName = this.selectedFile?.name || '';
    const fileNameWithoutExtension =
      fileName.substring(0, fileName.lastIndexOf('.')) || fileName;

    if (this.checkFile()) {
      this.createPreviewUrl();
      await this.handleImageCompression();
    }
  }

  fileValidator(control: AbstractControl): { [key: string]: any } | null {
    const file: File = control.value;

    if (!file) {
      return null;
    }

    if (!this.acceptedFileTypes.includes(file.type)) {
      return { invalidFileType: true };
    }

    if (file.size > this.maxFileSize) {
      return { invalidFileSize: true };
    }

    return null;
  }

  checkFile() {
    if (this.selectedFile) {
      if (!this.acceptedFileTypes.includes(this.selectedFile.type)) {
        this.fileErrorMessage =
          'Invalid file type, only JPEG, PNG, JPG, and GIF files are allowed';
        this.thumbnailFormGroup.controls.file.setErrors({
          invalidFileType: true,
        });
        return false;
      }
      if (this.selectedFile.size > this.maxFileSize) {
        this.fileErrorMessage = 'File size exceeds the maximum limit of 5MB';
        this.thumbnailFormGroup.controls.file.setErrors({
          invalidFileSize: true,
        });
        return false;
      }
      this.fileErrorMessage = '';
      this.thumbnailFormGroup.controls.file.setErrors(null);
      return true;
    }

    return false;
  }

  createPreviewUrl() {
    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.previewUrl = e.target?.result as string;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  onCancelSaveDialog() {
    this.showSaveDialog = false;
  }

  onSaveThumbnail() {
    if (this.checkFile()) {
      this.showSaveDialog = true;
      this.saveDialogMessage = 'Are you sure you want to save this thumbnail?';
    }
  }
  saveThumbnail() {
    console.log('Selected file', this.selectedFile);
    const request: UploadCourseThumbnailRequest = {
      courseId: this.courseId,
      useAzureStorage: this.thumbnailFormGroup.controls.useAzureStorage
        .value as boolean,
      thumbnailData: this.selectedFile as File,
    };

    this.uploadInProgress = true;
    this.uploadProgress = 0;
    this.uploadStatusMessage = 'Preparing to upload...';
    this.showSaveDialog = false;
    this.courseTutorControllerService.uploadCourseThumbnail(request).subscribe({
      next: (response) => {
        this.uploadProgress = response.progress;

        if (this.uploadProgress < 100) {
          this.uploadStatusMessage = `Uploading ${this.uploadProgress}% complete`;
        } else if (response.response) {
          this.uploadStatusMessage = 'Upload complete! Processing...';
        }

        if (response.response) {
          this.uploadInProgress = false;

          this.snackboxService.showSnackbox(
            'Thumbnail uploaded successfully',
            'success'
          );
        }
      },
      error: (error) => {
        this.snackboxService.showSnackbox(
          'Failed to upload thumbnail',
          'error'
        );
      },
    });
  }

  goBack() {
    this.router.navigate(['/tutor/course/', this.courseId]);
  }

  onRemoveThumbnail() {
    this.showRemoveDialog = true;
  }

  onCancelRemoveDialog() {
    this.showRemoveDialog = false;
  }

  removeThumbnail() {
    this.courseTutorControllerService
      .deleteCourseThumbnail(this.courseId)
      .subscribe({
        next: (response) => {
          this.snackboxService.showSnackbox(
            'Thumbnail removed successfully',
            'success'
          );
          this.showRemoveDialog = false;
          this.router.navigate(['/tutor/course/', this.courseId]);
        },
        error: (error) => {
          this.snackboxService.showSnackbox(
            'Failed to remove thumbnail',
            'error'
          );
        },
      });
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

  async onDrop(event: DragEvent): Promise<void> {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      this.isNewOrExisting = true;
      this.selectedFile = event.dataTransfer.files[0];

      if (this.checkFile()) {
        this.createPreviewUrl();
        await this.handleImageCompression();
      }
      event.dataTransfer.clearData();
    }
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

  urlValidator(control: AbstractControl) {
    const url = control.value;
    if (!url) return null;
    try {
      new URL(url);
      return null;
    } catch {
      return { invalidUrl: true };
    }
  }

}
