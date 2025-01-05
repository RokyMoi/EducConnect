import { Component, Input, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TextInputComponentComponent } from '../../../common/input/text/text-input-component/text-input-component.component';
import { TextAreaInputComponentComponent } from '../../../common/input/text/text-area-input-component/text-area-input-component.component';
import { ReferenceService } from '../../../services/reference/reference.service';

@Component({
  standalone: true,
  selector: 'app-course-basic-information',
  imports: [
    ReactiveFormsModule,
    TextInputComponentComponent,
    TextAreaInputComponentComponent,
  ],
  templateUrl: './course-basic-information.component.html',
  styleUrl: './course-basic-information.component.css',
})
export class CourseBasicInformationComponent implements OnInit {
  @Input() componentTitle: string = 'Enter basic information this course';
  @Input() referenceService!: ReferenceService;

  //Variables for the course name
  courseNameLabel: string = 'Course Name';
  courseNamePlaceholder: string = 'Enter the course name';
  courseNameErrorMessage: string = '';

  //Variables for the course subject
  courseSubjectLabel: string = 'Course Subject';
  courseSubjectPlaceholder: string =
    'Tell the students what is this course about';
  courseSubjectErrorMessage: string = '';

  //Variables for the course description
  courseDescriptionLabel: string = 'Course Description';
  courseDescriptionPlaceholder: string =
    'Describe in detail what this course is about';
  courseDescriptionErrorMessage: string = '';
  courseBasicInformationFormGroup = new FormGroup({
    courseName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(100),
    ]),
    courseSubject: new FormControl(
      '',

      [Validators.required, Validators.minLength(10), Validators.maxLength(200)]
    ),
    courseDescription: new FormControl(
      '',

      [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(1024),
      ]
    ),
  });

  ngOnInit(): void {
    this.referenceService
      .getLearningCategoriesAndSubcategories()
      .subscribe((response) => {
        console.log(response);
      });
  }
}
