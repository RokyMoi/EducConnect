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

@Component({
  selector: 'app-course-tutor-teaching-resources-details',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './course-tutor-teaching-resources-details.component.html',
  styleUrl: './course-tutor-teaching-resources-details.component.css',
})
export class CourseTutorTeachingResourcesDetailsComponent {
  courseId: string | null = null;

  isNewOrExisting: boolean = true;

  selectedFile: File | null = null;
  previewUrl: string | null = null;

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

  titleErrorMessage: string = '';
  descriptionErrorMessage: string = '';
  urlErrorMessage: string = '';
  courseTeachingResourceFormGroup: FormGroup = new FormGroup({
    resourceType: new FormControl(false),
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
    resourceLink: new FormControl(null),
  });
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
  }
  goBack() {
    this.router.navigate(['/tutor/course/teaching-resources/' + this.courseId]);
  }

  
}
