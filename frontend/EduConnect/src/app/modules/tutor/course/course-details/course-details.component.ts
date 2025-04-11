import { Component } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CreateCourseRequest } from '../../../../models/course/course-tutor-controller/create-course-request';
import { GetAllCourseCategoriesResponse } from '../../../../models/shared/reference-controller/get-all-course-categories';
import { GetAllLearningDifficultyLevelsResponse } from '../../../../models/shared/reference-controller/get-all-learning-difficulty-levels-response';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { ReferenceService } from '../../../../services/reference/reference.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { CommonModule } from '@angular/common';
import { GetAllCoursesResponse } from '../../../../models/course/course-tutor-controller/get-all-courses-response';
import { UpdateCourseBasicsRequest } from '../../../../models/course/course-tutor-controller/update-course-basics-request';

@Component({
  standalone: true,
  selector: 'app-course-details',
  imports: [
    CustomHeaderNgContentDialogBoxComponent,
    CommonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './course-details.component.html',
  styleUrl: './course-details.component.css',
})
export class CourseDetailsComponent {
  courseId: string | null = null;

  course: GetAllCoursesResponse | null = null;

  courseCategories: GetAllCourseCategoriesResponse[] = [];
  learningDifficultyLevels: GetAllLearningDifficultyLevelsResponse[] = [];

  titleErrorMessage: string = '';
  descriptionErrorMessage: string = '';
  courseCategoryErrorMessage: string = '';
  learningDifficultyLevelErrorMessage: string = '';
  priceErrorMessage: string = '';
  minimumErrorMessage: string = '';
  maximumErrorMessage: string = '';

  courseFormGroup: FormGroup = new FormGroup({
    title: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(255),
      ],
    }),
    description: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(15),
        Validators.maxLength(255),
      ],
    }),
    selectedCourseCategoryId: new FormControl('', {
      validators: [Validators.required],
    }),
    selectedLearningLevelId: new FormControl('', {
      validators: [Validators.required],
    }),
    price: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.min(0),
        Validators.max(999999999999999.99),
      ],
    }),
    minNumberOfStudents: new FormControl(null, {
      validators: [Validators.min(1), Validators.max(1000000000)],
    }),
    maxNumberOfStudents: new FormControl(null, {
      validators: [Validators.min(1), Validators.max(1000000000)],
    }),
  });

  showCreateCourseDialog: boolean = false;
  createCourseDialogMessage: string = '';

  showDiscardChangesCourseDialog: boolean = false;
  discardChangesCourseDialogMessage: string = '';
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private referenceControllerService: ReferenceService,
    private courseTutorControllerService: CourseTutorControllerService,
    private snackboxService: SnackboxService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
  }
  ngOnInit(): void {
    this.referenceControllerService.getAllCourseCategories().subscribe({
      next: (response) => {
        console.log(response);
        this.courseCategories = response.data;
      },
      error: (error) => {
        const errorMessage = error.error.message ? error.error.message : '';
        this.snackboxService.showSnackbox(
          'Failed to load course categories, ' + errorMessage,
          'error'
        );
      },
    });

    this.referenceControllerService.getAllLearningDifficultyLevels().subscribe({
      next: (response) => {
        console.log(response);
        this.learningDifficultyLevels = response.data;
        console.log(this.learningDifficultyLevels);
      },
      error: (error) => {
        const errorMessage = error.error.message ? error.error.message : '';
        this.snackboxService.showSnackbox(
          'Failed to load learning difficulty levels, ' + errorMessage,
          'error'
        );
      },
    });

    this.courseFormGroup.controls['title'].valueChanges.subscribe((value) => {
      this.checkForTitleError();

      if (
        this.courseFormGroup.controls['title'].valid &&
        this.courseFormGroup.controls['title'].touched
      ) {
        this.courseTutorControllerService
          .checkCourseTitleExistEmitGivenCourse({
            title: value,
            courseId: this.courseId as string,
          })
          .subscribe((response) => {
            if (!response.data) {
              this.courseFormGroup.controls['title'].setErrors(null);
              this.titleErrorMessage = '';
            } else {
              this.courseFormGroup.controls['title'].setErrors({
                titleTaken: true,
              });
              this.titleErrorMessage = 'This title is already taken';
            }
          });
      }
    });

    this.courseFormGroup.controls['description'].valueChanges.subscribe(
      (value) => {
        this.checkForDescriptionError();
      }
    );

    this.courseFormGroup.controls[
      'selectedCourseCategoryId'
    ].valueChanges.subscribe((value) => {
      this.checkForCategoryError();
    });

    this.courseFormGroup.controls[
      'selectedLearningLevelId'
    ].valueChanges.subscribe((value) => {
      this.checkForLevelError();
    });

    this.courseFormGroup.controls['price'].valueChanges.subscribe((value) => {
      this.checkForPriceError();
    });

    this.courseFormGroup.controls['minNumberOfStudents'].valueChanges.subscribe(
      (value) => {
        this.checkForMinNumberOfStudentsError();
      }
    );

    this.courseFormGroup.controls['maxNumberOfStudents'].valueChanges.subscribe(
      (value) => {
        this.checkForMaxNumberOfStudentsError();
      }
    );

    this.loadCourse();
  }

  goBack() {
    this.router.navigate(['/tutor/course/' + this.courseId]);
  }

  onSaveAction() {
    if (!this.checkForErrors()) {
      return;
    }
    this.showCreateCourseDialog = true;
    this.createCourseDialogMessage =
      'Are you sure you want to update the course with title: ' +
      this.courseFormGroup.controls['title'].value +
      '?';
  }

  checkForErrors() {
    this.checkForTitleError();
    this.checkForDescriptionError();
    this.checkForCategoryError();
    this.checkForLevelError();
    this.checkForPriceError();
    this.checkForMinNumberOfStudentsError();
    this.checkForMaxNumberOfStudentsError();

    if (this.courseFormGroup.invalid) {
      this.snackboxService.showSnackbox(
        'Please fix the errors in the form, before saving',
        'error'
      );
      return false;
    }

    return true;
  }

  checkForTitleError() {
    const titleError = this.courseFormGroup.controls['title'].errors;
    if (titleError) {
      if (titleError['required']) {
        this.titleErrorMessage = 'Title is required';
      } else if (titleError['minlength']) {
        this.titleErrorMessage = 'Title must be at least 2 characters long';
      } else if (titleError['maxlength']) {
        this.titleErrorMessage = 'Title must be at most 255 characters long';
      }
      return;
    }
    this.titleErrorMessage = '';
  }

  checkForDescriptionError() {
    const descriptionErrors =
      this.courseFormGroup.controls['description'].errors;
    if (descriptionErrors) {
      if (descriptionErrors['required']) {
        this.descriptionErrorMessage = 'Title is required';
      } else if (descriptionErrors['minlength']) {
        this.descriptionErrorMessage =
          'Title must be at least 2 characters long';
      } else if (descriptionErrors['maxlength']) {
        this.descriptionErrorMessage =
          'Title must be at most 255 characters long';
      }
      return;
    }
    this.descriptionErrorMessage = '';
  }

  checkForCategoryError() {
    const categoryError =
      this.courseFormGroup.controls['selectedCourseCategoryId'].errors;
    if (categoryError) {
      this.courseCategoryErrorMessage = 'This field is required';
      return;
    }
    this.courseCategoryErrorMessage = '';
  }

  checkForLevelError() {
    const levelError =
      this.courseFormGroup.controls['selectedLearningLevelId'].errors;
    if (levelError) {
      this.learningDifficultyLevelErrorMessage = 'This field is required';
      return;
    }
    this.learningDifficultyLevelErrorMessage = '';
  }

  checkForPriceError() {
    const priceError = this.courseFormGroup.controls['price'].errors;
    if (priceError) {
      if (priceError['required']) {
        this.priceErrorMessage = 'Price is required';
      } else if (priceError['min']) {
        this.priceErrorMessage = 'Price must be at least 0';
      } else if (priceError['max']) {
        this.priceErrorMessage = 'Price must be at most 999999999999999.99';
      }
      return;
    }
    this.priceErrorMessage = '';
  }

  checkForMinNumberOfStudentsError() {
    const minErrors =
      this.courseFormGroup.controls['minNumberOfStudents'].errors;
    console.log(minErrors);
    if (minErrors) {
      if (minErrors['min']) {
        this.minimumErrorMessage =
          'Minimum number of students must be at least 1';
      } else if (minErrors['max']) {
        this.minimumErrorMessage =
          'Minimum number of students must be at most 1000000000';
      }
      return;
    }
    this.minimumErrorMessage = '';
  }

  checkForMaxNumberOfStudentsError() {
    const maxErrors =
      this.courseFormGroup.controls['maxNumberOfStudents'].errors;
    if (maxErrors) {
      if (maxErrors['min']) {
        this.maximumErrorMessage =
          'Maximum number of students must be at least 1';
      } else if (maxErrors['max']) {
        this.maximumErrorMessage =
          'Maximum number of students must be at most 1000000000';
      }
      return;
    }

    if (
      this.courseFormGroup.controls['maxNumberOfStudents'].value &&
      this.courseFormGroup.controls['minNumberOfStudents'].value &&
      Number(this.courseFormGroup.controls['maxNumberOfStudents'].value) <
        Number(this.courseFormGroup.controls['minNumberOfStudents'].value)
    ) {
      this.maximumErrorMessage =
        'Maximum number of students must be greater than minimum number of students';
      this.courseFormGroup.setErrors({
        maxLessThanMin: true,
      });
      return;
    }
    if (
      this.courseFormGroup.controls['maxNumberOfStudents'].value &&
      this.courseFormGroup.controls['minNumberOfStudents'].value &&
      Number(this.courseFormGroup.controls['maxNumberOfStudents'].value) >
        Number(this.courseFormGroup.controls['minNumberOfStudents'].value)
    ) {
      this.courseFormGroup.setErrors(null);
    }
    console.log(this.courseFormGroup.controls['maxNumberOfStudents'].errors);
    this.maximumErrorMessage = '';
  }

  onCancelSaveDialog() {
    this.showCreateCourseDialog = false;
  }

  saveCourse() {
    console.log('Course saved');

    const request: UpdateCourseBasicsRequest = {
      courseId: this.courseId as string,
      title: this.courseFormGroup.controls['title'].value,
      description: this.courseFormGroup.controls['description'].value,
      courseCategoryId:
        this.courseFormGroup.controls['selectedCourseCategoryId'].value,
      learningDifficultyLevelId:
        this.courseFormGroup.controls['selectedLearningLevelId'].value,
      price: this.courseFormGroup.controls['price'].value,
      minNumberOfStudents:
        this.courseFormGroup.controls['minNumberOfStudents'].value,
      maxNumberOfStudents:
        this.courseFormGroup.controls['maxNumberOfStudents'].value,
    };

    this.courseTutorControllerService.updateCourseBasics(request).subscribe({
      next: (response) => {
        console.log(response);
        this.showCreateCourseDialog = false;
        this.snackboxService.showSnackbox(
          'Course updated successfully',
          'success'
        );
        this.loadCourse();
      },
      error: (error) => {
        console.log(error);
        this.showCreateCourseDialog = false;
        this.snackboxService.showSnackbox(
          'Error updating course: ' + error.error.message,
          'error'
        );
      },
    });
  }

  loadCourse() {
    this.courseTutorControllerService
      .getCourseBasics(this.courseId as string)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.course = response.data;
          this.courseFormGroup.patchValue({
            title: this.course?.title,
            description: this.course?.description,
            selectedCourseCategoryId: this.course?.courseCategoryId,
            selectedLearningLevelId: this.course?.learningDifficultyLevelId,
            price: this.course?.price,
            minNumberOfStudents: this.course?.minNumberOfStudents,
            maxNumberOfStudents: this.course?.maxNumberOfStudents,
          });
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  onDiscard() {
    this.discardChangesCourseDialogMessage =
      'Are you sure you want to discard changes?';
    this.showDiscardChangesCourseDialog = true;
  }
  discardChanges() {
    this.showDiscardChangesCourseDialog = false;
    this.loadCourse();
  }
  onCancelDiscardChangesDialog() {
    this.showDiscardChangesCourseDialog = false;
  }
}
