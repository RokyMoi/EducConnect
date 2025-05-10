import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PromotionService } from '../../services/promotion/course-promotion.service';
import { CoursePromotionDetail, PromotionStatus } from '../../models/promotionCourse/promotion.model';
import { forkJoin } from 'rxjs';
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CourseLoadService } from '../../services/promotion/course-load.service';
import { CommonModule } from '@angular/common';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import {MatCard, MatCardActions, MatCardContent, MatCardTitle} from '@angular/material/card';

@Component({
  selector: 'app-promotion-form',
  templateUrl: './promotion-form.component.html',
  styleUrls: ['./promotion-form.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatOptionModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinner,
    MatCardActions,
    MatCardContent,
    MatCardTitle,
    MatCard
  ],
  providers: [
    provideNativeDateAdapter()
  ]
})
export class PromotionFormComponent implements OnInit {
  promotionForm!: FormGroup;
  courses: any;
  isEditMode = false;
  promotionId: string | null = null;
  loading = true;
  submitting = false;
  uploadedFiles: File[] = [];
  existingImages: any[] = [];
  imagesToRemove: string[] = [];
  mainImageId?: string;
  PromotionStatus = PromotionStatus;
  formError: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private promotionService: PromotionService,
    private courseLoadService: CourseLoadService,
  ) { }

  ngOnInit(): void {
    this.initForm();

    this.promotionId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.promotionId;

    const coursesObservable = this.courseLoadService.getAllCourses();

    if (this.isEditMode && this.promotionId) {
      forkJoin({
        courses: coursesObservable,
        promotion: this.promotionService.getPromotionById(this.promotionId)
      }).subscribe({
        next: ({ courses, promotion }) => {
          this.courses = courses;
          this.populateForm(promotion);
          this.existingImages = promotion.images.map((img:any) => ({
            ...img,
            url: this.promotionService.getImageUrl(img.imageId)
          }));
          this.mainImageId = promotion.images.find((img:any) => img.isMainImage)?.imageId;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading data', error);
          this.snackBar.open('Error loading promotion data', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    } else {
      coursesObservable.subscribe({
        next: (courses:any) => {
          this.courses = courses;
          this.loading = false;
        },
        error: (error:any) => {
          console.error('Error loading courses', error);
          this.snackBar.open('Error loading courses', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    }
  }

  initForm(): void {
    this.promotionForm = this.fb.group({
      courseId: ['', Validators.required],
      title: ['', [Validators.required, Validators.maxLength(500)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      status: [PromotionStatus.Draft]
    });
  }

  populateForm(promotion: CoursePromotionDetail): void {
    let startDate = '';
    let endDate = '';

    if (promotion.startDate) {
      const start = new Date(promotion.startDate * 1000);
      startDate = start.toISOString().substring(0, 10);
    }

    if (promotion.endDate) {
      const end = new Date(promotion.endDate * 1000);
      endDate = end.toISOString().substring(0, 10);
    }

    this.promotionForm.patchValue({
      courseId: promotion.courseId,
      title: promotion.title,
      description: promotion.description,
      startDate: startDate,
      endDate: endDate,
      status: promotion.status
    });
  }

  onFileSelected(event: any): void {
    const files = event.target.files;
    if (files) {
      this.uploadedFiles = [...this.uploadedFiles, ...Array.from(files) as File[]];
    }
  }

  removeUploadedFile(index: number): void {
    this.uploadedFiles = this.uploadedFiles.filter((_, i) => i !== index);
  }

  removeExistingImage(imageId: string): void {
    this.existingImages = this.existingImages.filter(img => img.imageId !== imageId);
    this.imagesToRemove.push(imageId);

    if (this.mainImageId === imageId) {
      this.mainImageId = this.existingImages.length > 0 ? this.existingImages[0].imageId : undefined;
    }
  }

  setMainImage(imageId: string): void {
    this.mainImageId = imageId;
  }

  validateFormData(): boolean {
    this.formError = null;

    if (this.promotionForm.invalid) {
      // Get specific validation errors for debugging
      const errors: string[] = [];

      if (this.promotionForm.get('courseId')?.errors) {
        errors.push('Course selection is required');
      }

      if (this.promotionForm.get('title')?.errors) {
        if (this.promotionForm.get('title')?.hasError('required')) {
          errors.push('Title is required');
        } else if (this.promotionForm.get('title')?.hasError('maxlength')) {
          errors.push('Title exceeds maximum length');
        }
      }

      if (this.promotionForm.get('description')?.errors) {
        if (this.promotionForm.get('description')?.hasError('required')) {
          errors.push('Description is required');
        } else if (this.promotionForm.get('description')?.hasError('maxlength')) {
          errors.push('Description exceeds maximum length');
        }
      }

      if (this.promotionForm.get('startDate')?.errors) {
        errors.push('Valid start date is required');
      }

      if (this.promotionForm.get('endDate')?.errors) {
        errors.push('Valid end date is required');
      }

      this.formError = errors.join(', ');
      this.snackBar.open(`Please fix the following issues: ${this.formError}`, 'Close', { duration: 5000 });
      return false;
    }

    return true;
  }

  onSubmit(): void {
    if (!this.validateFormData()) {
      return;
    }

    console.log('Submitting form with values:', this.promotionForm.value);
    console.log('Uploaded files:', this.uploadedFiles);

    this.submitting = true;
    const formValue = this.promotionForm.value;

    // Convert dates to timestamps
    const startTimestamp = formValue.startDate ?
      Math.floor(new Date(formValue.startDate).getTime() / 1000) : undefined;
    const endTimestamp = formValue.endDate ?
      Math.floor(new Date(formValue.endDate).getTime() / 1000) : undefined;

    if (this.isEditMode && this.promotionId) {
      const updateData = {
        promotionId: this.promotionId,
        title: formValue.title,
        description: formValue.description,
        status: formValue.status,
        startDate: startTimestamp,
        endDate: endTimestamp,
        mainImageId: this.mainImageId,
        removeImageIds: this.imagesToRemove.length > 0 ? this.imagesToRemove : undefined
      };

      this.promotionService.updatePromotion(updateData, this.uploadedFiles).subscribe({
        next: () => {
          this.snackBar.open('Promotion updated successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/promotions/list']);
        },
        error: (error: any) => {
          console.error('Error updating promotion', error);
          console.log('Error details:', error.error);
          this.snackBar.open('Error updating promotion: ' + (error.error?.message || error.message || 'Unknown error'), 'Close', { duration: 5000 });
          this.submitting = false;
        }
      });
    } else {
      const createData = {
        courseId: formValue.courseId,
        title: formValue.title,
        description: formValue.description,
        startDate: startTimestamp!,
        endDate: endTimestamp!
      };

      this.promotionService.createPromotion(createData, this.uploadedFiles).subscribe({
        next: (response) => {
          console.log('Create promotion response:', response);
          this.snackBar.open('Promotion created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/promotions/list']);
        },
        error: (error: any) => {
          console.error('Error creating promotion', error);
          console.log('Error details:', error.error);

          const errorMessage = error.error?.message ||
            (error.error?.errors ? JSON.stringify(error.error.errors) : null) ||
            error.message ||
            'Unknown error';

          this.snackBar.open('Error creating promotion: ' + errorMessage, 'Close', { duration: 5000 });
          this.submitting = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/promotions/list']);
  }
}
