 import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetAllCourseLessonsResponse } from '../../../../models/course/course-tutor-controller/get-all-course-lessons-response';
import { PublishedStatus } from '../../../../../enums/published-status.enum';

@Component({
  selector: 'app-course-tutor-lessons',
  standalone: true,
  imports: [CommonModule],  
  templateUrl: './course-tutor-lessons.component.html',
  styleUrl: './course-tutor-lessons.component.css',
})
export class CourseTutorLessonsComponent implements OnInit {
  courseId: string = '';

  lessons: GetAllCourseLessonsResponse[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService,
    private courseTutorControllerService: CourseTutorControllerService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId') as string;
  }

  ngOnInit(): void {
    this.loadLessons();
  }

  loadLessons() {
    this.courseTutorControllerService
      .getAllCourseLessons(this.courseId)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.lessons = response.data;
          this.lessons = response.data.sort(
            (a: GetAllCourseLessonsResponse, b: GetAllCourseLessonsResponse) =>
              (a.lessonSequenceOrder ?? Infinity) -
              (b.lessonSequenceOrder ?? Infinity)
          );
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            `Failed to load lessons for this course${
              error.error.message ? `: ${error.error.message}` : ''
            }`,
            'error'
          );
        },
      });
  }
  onCreateLesson() {
    this.router.navigate(['/tutor/course/lessons/new', this.courseId]);
  }
  onViewLesson(lesson: GetAllCourseLessonsResponse) {
    this.router.navigate([
      '/tutor/course/lessons/details',
      this.courseId,
      lesson.courseLessonId,
    ]);
  }

  toggleLessonStatus(lesson: GetAllCourseLessonsResponse) {
    // Toggle publish status logic
  }

  archiveLesson(lesson: GetAllCourseLessonsResponse) {
    // Archive lesson logic
  }

  goBack() {
    this.router.navigate(['/tutor/course', this.courseId]);
  }

  getPublishedStatus(publishedStatus: number): string {
    return PublishedStatus[publishedStatus];
  }

  onViewLessonResources(lessonId: string) {
    this.router.navigate([
      'tutor/course/lessons/resources',
      this.courseId,
      lessonId,
    ]);
  }
}
