import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CourseType } from '../../../../models/reference/course-type';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import { NgFor, NgIf } from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ReferenceService } from '../../../../services/reference/reference.service';
import { CourseCreateService } from '../../../../services/course/course-create-service.service';
import {
  MatProgressSpinnerModule,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';

@Component({
  standalone: true,
  selector: 'app-confirm-course-type',
  imports: [
    SubmitButtonComponent,
    ReactiveFormsModule,
    NgFor,
    MatProgressSpinnerModule,
    NgIf,
  ],
  templateUrl: './confirm-course-type.component.html',
  styleUrl: './confirm-course-type.component.css',
})
export class ConfirmCourseTypeComponent implements OnInit {
  @Input() courseId!: string;
  @Input() originalSelectedCourseType!: CourseType;
  selectedCourseType: CourseType = this.originalSelectedCourseType;

  @Input() referenceService!: ReferenceService;
  @Input() courseCreateService!: CourseCreateService;

  @Output() goToNextStep: EventEmitter<void> = new EventEmitter<void>();

  @Output() confirmCourseTypeStepCompleted: EventEmitter<boolean> =
    new EventEmitter<boolean>();

  courseTypeFormGroup = new FormGroup({
    selectedOption: new FormControl(0, [Validators.required]),
  });

  courseTypeArray: CourseType[] = [];

  confirmMyChoiceButtonText: string = 'Confirm My Choice';
  confirmMyChoiceButtonColor: string = 'green';
  confirmMyChoiceButtonDisabled: boolean = false;

  isDataTransmissionActive: boolean = false;
  isDataTransmissionComplete: boolean = false;

  confirmationServerResponseMessage: string = '';
  confirmationServerResponseMessageColor: string = '';
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';

  nextStepButtonText: string = 'Next Step';
  nextStepButtonColor: string = 'green';

  ngOnInit(): void {
    console.log(
      'The following values have been passed from the parent to the confirm-course-type component'
    );
    console.log('Course Id: ' + this.courseId);
    console.log(
      'Original selected course type: ',
      this.originalSelectedCourseType
    );
    console.log(
      'Selected course type taken from the originalSelectedCourseType: ',
      this.selectedCourseType
    );
    console.log('Course type array: ', this.courseTypeArray);

    this.selectedCourseType = this.originalSelectedCourseType;
    this.courseTypeFormGroup.controls.selectedOption.setValue(
      this.originalSelectedCourseType.courseTypeId
    );
    this.referenceService.getAllCourseTypes().subscribe((response) => {
      if (response.success === 'true') {
        this.courseTypeArray = response.data;
      }
    });
  }

  onCourseTypeChange(index: number) {
    this.selectedCourseType = this.courseTypeArray[index];
    this.courseTypeFormGroup.controls['selectedOption'].setValue(
      this.selectedCourseType.courseTypeId
    );
  }

  onConfirmMyChoiceButtonClick() {
    console.log(this.selectedCourseType);
    console.log('Is this the selected course type?');

    if (
      this.selectedCourseType.courseTypeId !==
      this.originalSelectedCourseType.courseTypeId
    ) {
      this.isDataTransmissionActive = true;
      this.isDataTransmissionComplete = false;
      this.confirmationServerResponseMessage = 'Confirming your choice...';
      this.confirmationServerResponseMessageColor = 'orange';

      this.courseCreateService
        .updateCourseTypeByCourseId(
          this.courseId,
          this.selectedCourseType.courseTypeId
        )
        .subscribe((response) => {
          this.isDataTransmissionActive = false;
          this.isDataTransmissionComplete = true;
          if (response.success === 'true') {
            this.confirmationServerResponseMessage =
              'Your choice has been saved, you can now proceed to the next step.';
            this.confirmationServerResponseMessageColor = 'green';
            this.confirmCourseTypeStepCompleted.emit(true);
          }
          if (response.success !== 'true') {
            this.confirmationServerResponseMessage =
              'We failed to save your choice, ' + response.message;
            this.confirmationServerResponseMessageColor = 'red';
          }
        });
    }

    if (
      this.selectedCourseType.courseTypeId ===
      this.originalSelectedCourseType.courseTypeId
    ) {
      this.isDataTransmissionComplete = false;
      this.isDataTransmissionComplete = true;
      this.confirmationServerResponseMessage =
        'You have not changed your choice, you can now proceed to the next step.';
      this.confirmationServerResponseMessageColor = 'green';
    }
  }

  onNextStepButtonClick() {
    this.goToNextStep.emit();
  }
}
