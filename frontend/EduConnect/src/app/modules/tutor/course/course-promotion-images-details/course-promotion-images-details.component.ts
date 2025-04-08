import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetCoursePromotionImageMetadataByIdResponse } from '../../../../models/course/course-tutor-controller/get-course-promotion-image-metadata-by-id-response';
import { FormControl } from '@angular/forms';
import { ImageCompressionService } from '../../../../services/image-compression.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { CommonModule } from '@angular/common';
import { UploadCoursePromotionImageRequest } from '../../../../models/course/course-tutor-controller/upload-course-promotion-image-request';

@Component({
  selector: 'app-course-promotion-images-details',
  standalone: true,
  imports: [CustomHeaderNgContentDialogBoxComponent, CommonModule],
  templateUrl: './course-promotion-images-details.component.html',
  styleUrl: './course-promotion-images-details.component.css',
})
export class CoursePromotionImagesDetailsComponent implements OnInit {
  courseId: string = '';
  coursePromotionImageId: string = '';

  existingImageMetadata: GetCoursePromotionImageMetadataByIdResponse | null =
    null;

  selectedFile: File | null = null;
  previewUrl: string = '';

  imageLink = '';

  acceptedFileTypes: string[] = [
    'image/jpeg',
    'image/png',
    'image/jpg',
    'image/gif',
  ];

  maxFileSize: number = 5 * 1024 * 1024;

  fileErrorMessage: string = '';

  showSaveDialog: boolean = false;
  saveDialogMessage: string = '';

  showRemoveDialog: boolean = false;

  uploadInProgress: boolean = false;
  uploadProgress: number = 50;
  uploadStatusMessage: string = '';

  isDragging: boolean = false;

  imageFileFormControl: FormControl = new FormControl(null);

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private courseTutorControllerService: CourseTutorControllerService,
    private imageCompressionService: ImageCompressionService,
    private snackboxService: SnackboxService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId') as string;

    this.coursePromotionImageId = this.route.snapshot.paramMap.get(
      'imageId'
    ) as string;

    console.log('courseId:', this.courseId);
    console.log('coursePromotionImageId:', this.coursePromotionImageId);
  }
  ngOnInit(): void {
    if (
      this.coursePromotionImageId &&
      this.coursePromotionImageId.trim().length > 0
    ) {
      this.fetchImage();
    }
  }

  goBack() {
    this.router.navigate(['/tutor/course/promotion', this.courseId]);
  }

  fetchImage() {
    this.courseTutorControllerService
      .getPromotionImage(this.coursePromotionImageId)
      .subscribe({
        next: (response) => {
          console.log('Image:', response);
          this.existingImageMetadata = response.data;
          this.getImageLink(
            this.existingImageMetadata?.coursePromotionImageId as string
          );
        },
        error: (error) => {
          console.error('Error fetching image:', error);
        },
      });
  }

  getComponentTitle() {
    return this.existingImageMetadata
      ? `Promotion Image Details For ${this.existingImageMetadata.courseTitle}`
      : 'New Course Promotion Image';
  }

  getImageLink(imageId: string) {
    return `http://localhost:5177/public/course/promotion/image/${imageId}?t=${Date.now()}`;
  }

  async onFileSelected(event: any) {
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

  checkFile() {
    if (this.selectedFile) {
      if (!this.acceptedFileTypes.includes(this.selectedFile.type)) {
        this.fileErrorMessage =
          'Invalid file type, only JPEG, PNG, JPG, and GIF files are allowed';
        this.imageFileFormControl.setErrors({
          invalidFileType: true,
        });
        return false;
      }
      if (this.selectedFile.size > this.maxFileSize) {
        this.fileErrorMessage = 'File size exceeds the maximum limit of 5MB';
        this.imageFileFormControl.setErrors({
          invalidFileSize: true,
        });
        return false;
      }
      this.fileErrorMessage = '';
      this.imageFileFormControl.setErrors(null);
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

  onCancelSaveDialog() {
    this.showSaveDialog = false;
    this.saveDialogMessage = '';
  }

  onSaveImage() {
    console.log('Save Image');
    if (this.selectedFile) {
      this.showSaveDialog = true;
      this.saveDialogMessage = `Are you sure you want to ${
        this.existingImageMetadata
          ? 'save changes to the image'
          : 'upload this image'
      }?`;
    }
  }

  saveImage() {
    const request: UploadCoursePromotionImageRequest = {
      courseId: this.courseId,
      coursePromotionImageId: this.existingImageMetadata
        ? this.coursePromotionImageId
        : null,
      image: this.selectedFile as File,
    };

    this.uploadInProgress = true;
    this.uploadProgress = 0;
    this.uploadStatusMessage = 'Preparing to upload...';
    this.showSaveDialog = false;

    this.courseTutorControllerService
      .uploadCoursePromotionImage(request)
      .subscribe({
        next: (response) => {
          this.uploadProgress = response.progress;

          if (this.uploadProgress < 100) {
            this.uploadStatusMessage = `Uploading ${this.uploadProgress}% complete`;
          } else if (response.response) {
            this.uploadStatusMessage = 'Upload complete! Processing...';
          }

          if (response.response) {
            this.uploadInProgress = false;
            if (response.response.data) {
              this.coursePromotionImageId = response.response.data;
            }

            this.fetchImage();
            this.snackboxService.showSnackbox(
              'Image uploaded successfully',
              'success'
            );
          }
        },
        error: (error) => {
          this.snackboxService.showSnackbox(
            `Failed to upload image${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  onCancelRemoveDialog() {
    this.showRemoveDialog = false;
  }

  onRemoveImage() {
    this.showRemoveDialog = true;
  }

  removeImage() {
    this.showRemoveDialog = true;
    this.courseTutorControllerService
      .deleteCoursePromotionImage(this.coursePromotionImageId)
      .subscribe({
        next: (response) => {
          console.log('IImage deleted successfully');
          this.snackboxService.showSnackbox(
            'Image deleted successfully',
            'success'
          );
          this.router.navigate(['/tutor/course/promotion', this.courseId]);
        },
        error: (error) => {
          console.log('Failed to delete image:', error);
          this.snackboxService.showSnackbox('Failed to delete image', 'error');
        },
      });
  }
}
