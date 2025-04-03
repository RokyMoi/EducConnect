import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import {
  Editor,
  NgxEditorModule,
  Validators as ngxValidators,
  toDoc,
} from 'ngx-editor';
import { CustomHeaderNgContentDialogBoxComponent } from '../../../shared/custom-header-ng-content-dialog-box/custom-header-ng-content-dialog-box.component';
import { CreateCourseLessonRequest } from '../../../../models/course/course-tutor-controller/create-course-lesson-request';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetAllCourseLessonsResponse } from '../../../../models/course/course-tutor-controller/get-all-course-lessons-response';
import { PublishedStatus } from '../../../../../enums/published-status.enum';
@Component({
  selector: 'app-course-tutor-lesson-details',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgxDocViewerModule,
    NgxEditorModule,
    CustomHeaderNgContentDialogBoxComponent,
  ],
  templateUrl: './course-tutor-lesson-details.component.html',
  styleUrl: './course-tutor-lesson-details.component.css',
})
export class CourseTutorLessonDetailsComponent implements OnInit {
  courseId: string | null = null;
  courseLessonId: string | null = null;
  editorContent: string = '';

  existingLesson: GetAllCourseLessonsResponse | null = null;
  public editor: Editor = new Editor();

  showSaveDialog: boolean = false;
  saveDialogMessage: string = '';

  showPublishDialog: boolean = false;
  publishDialogMessage: string = '';

  showArchiveDialog: boolean = false;
  archiveDialogMessage: string = '';

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
    description: new FormControl(this.editorContent, {
      validators: [
        Validators.required,
        Validators.minLength(70),
        Validators.maxLength(1000),
      ],
    }),
    content: new FormControl(null, {
      validators: [
        ngxValidators.required(),
        ngxValidators.minLength(100),
        ngxValidators.maxLength(100000),
      ],
    }),
    lessonSequenceOrder: new FormControl(null, {
      validators: [Validators.min(1)],
    }),
    publishedStatus: new FormControl(null),
  });
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService,
    private courseTutorControllerService: CourseTutorControllerService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
    this.courseLessonId = this.route.snapshot.paramMap.get('lessonId');
  }
  ngOnInit(): void {
    if (this.courseLessonId) {
      this.fetchLesson();
    }
    this.lessonForm.controls['content'].valueChanges.subscribe((value) => {
      console.log(this.lessonForm.controls['content'].errors);
    });
    console.log(this.lessonForm.errors);
    console.log(this.lessonForm.value);
  }
  goBack() {
    this.router.navigate(['/tutor/course/lessons/', this.courseId]);
  }

  onSaveLesson() {
    this.saveDialogMessage = 'Are you sure you want to save this lesson?';
    this.showSaveDialog = true;
  }

  onCancelSaveDialog() {
    this.showSaveDialog = false;
  }

  saveLesson() {
    console.log(this.lessonForm.controls['content'].value);

    const request: CreateCourseLessonRequest = {
      courseId: this.courseId as string,
      courseLessonId: this.courseLessonId ? this.courseLessonId : null,
      title: this.lessonForm.controls['title'].value,
      shortSummary: this.lessonForm.controls['shortSummary'].value,
      description: this.lessonForm.controls['description'].value,
      topic: this.lessonForm.controls['topic'].value,
      content: this.lessonForm.controls['content'].value,
      lessonSequenceOrder:
        this.lessonForm.controls['lessonSequenceOrder'].value,
    };
    console.log(request);

    this.courseTutorControllerService
      .createOrUpdateCourseLesson(request)
      .subscribe({
        next: (response) => {
          this.showSaveDialog = false;
          this.snackboxService.showSnackbox(
            'Lesson saved successfully',
            'success'
          );
          this.router.navigate(['/tutor/course/', this.courseId]);
        },
        error: (error) => {
          this.showSaveDialog = false;
          this.snackboxService.showSnackbox(
            `Error saving lesson: ${
              error.message.message ? `, ${error.message.message}` : ''
            }`,
            'error'
          );
          console.log(error);
        },
      });
  }

  convertNgxEditorJsonToHtml(content: any): string {
    if (!content || !content.content) return '';

    let html = '';

    content.content.forEach((block: any) => {
      if (block.type === 'heading' && block.content) {
        html += `<h${block.attrs.level}>${this.extractTextFromContent(
          block.content
        )}</h${block.attrs.level}>`;
      } else if (block.type === 'paragraph' && block.content) {
        html += `<p>${this.extractTextFromContent(block.content)}</p>`;
      }
    });

    return html;
  }

  // Helper function to extract text content
  extractTextFromContent(contentArray: any[]): string {
    return contentArray.map((node) => (node.text ? node.text : '')).join('');
  }

  fetchLesson() {
    this.courseTutorControllerService
      .getCourseLessonById(this.courseLessonId as string)
      .subscribe({
        next: (response) => {
          this.existingLesson = response.data;

          this.lessonForm.patchValue({
            title: this.existingLesson?.title,
            topic: this.existingLesson?.topic,
            shortSummary: this.existingLesson?.shortSummary,
            description: this.existingLesson?.description,
            content: this.existingLesson?.courseLessonContent,
            lessonSequenceOrder: this.existingLesson?.lessonSequenceOrder,
            publishedStatus: this.existingLesson?.publishedStatus,
          });
          this.editorContent = this.existingLesson
            ?.courseLessonContent as string;

          console.log('Response:', response);
          console.log('Existing Lesson:', this.existingLesson);
          console.log('Editor Content:', this.editorContent);
          console.log('Form Value:', this.lessonForm.value);
        },
        error: (error) => {
          this.snackboxService.showSnackbox(
            `Error fetching lesson: ${
              error.message.message ? `, ${error.message.message}` : ''
            }`,
            'error'
          );
          console.log(error);
        },
      });
  }

  get getPublishedStatus() {
    return this.existingLesson
      ? PublishedStatus[this.existingLesson?.publishedStatus]
      : '';
  }

  onPublishLesson() {
    if (this.lessonForm.controls['lessonSequenceOrder'].value === null) {
      this.snackboxService.showSnackbox(
        'Please add lesson sequence order',
        'error'
      );
      return;
    }
    this.publishDialogMessage = 'Are you sure you want to publish this lesson?';
    this.showPublishDialog = true;
    console.log('Before publishing:', this.existingLesson);
  }

  onCancelPublishDialog() {
    this.showPublishDialog = false;
  }

  onArchiveLesson() {
    this.archiveDialogMessage = 'Are you sure you want to archive this lesson?';
    this.showArchiveDialog = true;
    console.log('Before archiving:', this.existingLesson);
  }

  onCancelArchiveDialog() {
    this.showArchiveDialog = false;
  }

  publishLesson() {
    console.log('Publishing lesson: ', this.existingLesson);
    this.showPublishDialog = false;
    this.courseTutorControllerService
      .publishCourseLesson({
        courseLessonId: this.courseLessonId as string,
        lessonSequenceOrder:
          this.lessonForm.controls['lessonSequenceOrder'].value,
      })
      .subscribe({
        next: (response) => {
          console.log('Lesson published successfully');
          this.snackboxService.showSnackbox(
            'Lesson published successfully',
            'success'
          );
          this.fetchLesson();
        },
        error: (error) => {
          console.log('Failed to publish lesson:', error);
          this.snackboxService.showSnackbox(
            `Error publishing lesson: ${
              error.error.message ? `, ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }

  archiveLesson() {
    console.log('Archiving lesson: ', this.existingLesson);
    this.showArchiveDialog = false;
    this.courseTutorControllerService
      .archiveCourseLesson(this.courseLessonId as string)
      .subscribe({
        next: (response) => {
          console.log('Lesson archived successfully');
          this.snackboxService.showSnackbox(
            'Lesson archived successfully',
            'success'
          );
          this.fetchLesson();
        },
        error: (error) => {
          console.log('Failed to archive lesson:', error);
          this.snackboxService.showSnackbox(
            `Error archiving lesson${
              error.error.message ? `, ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }
}
