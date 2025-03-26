import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { Editor, NgxEditorModule } from 'ngx-editor';
@Component({
  selector: 'app-course-tutor-lesson-details',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgxDocViewerModule,
    NgxEditorModule,
  ],
  templateUrl: './course-tutor-lesson-details.component.html',
  styleUrl: './course-tutor-lesson-details.component.css',
})
export class CourseTutorLessonDetailsComponent {
  courseId: string | null = null;
  lessonId: string | null = null;

  public editor: Editor = new Editor();
  
  titleErrorMessage: string = '';
  topicErrorMessage: string = '';
  shortSummaryErrorMessage: string = '';
  descriptionErrorMessage: string = '';
  lessonForm: FormGroup = new FormGroup({
    title: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(15),
        Validators.maxLength(50),
      ],
    }),
    topic: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(100),
      ],
    }),
    shortSummary: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(45),
        Validators.maxLength(250),
      ],
    }),
    description: new FormControl(null, {
      validators: [
        Validators.required,
        Validators.minLength(70),
        Validators.maxLength(1000),
      ],
    }),
    content: new FormControl(null, [
      Validators.required,
      Validators.minLength(100),
      Validators.maxLength(100000),
    ]),
    lessonSequenceOrder: new FormControl(null, {
      validators: [Validators.min(1)],
    }),
    publishedStatus: new FormControl(null),
  });
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
  }
  goBack() {
    this.router.navigate(['/tutor/course/', this.courseId]);
  }
}
