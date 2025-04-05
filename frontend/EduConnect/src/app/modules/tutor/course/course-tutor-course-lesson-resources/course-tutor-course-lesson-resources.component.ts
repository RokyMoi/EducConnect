import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { GetAllCourseLessonResourcesResponse } from '../../../../models/course/course-tutor-controller/get-all-course-lesson-resources-response';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackboxService } from '../../../../services/shared/snackbox.service';

@Component({
  selector: 'app-course-tutor-course-lesson-resources',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-tutor-course-lesson-resources.component.html',
  styleUrl: './course-tutor-course-lesson-resources.component.css',
})
export class CourseTutorCourseLessonResourcesComponent implements OnInit {
  resources: GetAllCourseLessonResourcesResponse[] = [];
  courseLessonId: string = '';
  courseId: string = '';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private snackboxService: SnackboxService,
    private courseTutorControllerService: CourseTutorControllerService
  ) {}
  ngOnInit(): void {
    this.courseId = this.route.snapshot.paramMap.get('courseId') as string;
    this.courseLessonId = this.route.snapshot.paramMap.get(
      'lessonId'
    ) as string;
    this.fetchResources();
  }

  goBack() {
    this.router.navigate([
      '/tutor/course/lessons/details',
      this.courseId,
      this.courseLessonId,
    ]);
  }
  onAddNewResource() {
    this.router.navigate([
      'tutor/course/lessons/resources/new',
      this.courseId,
      this.courseLessonId,
    ]);
  }
  fetchResources() {
    this.courseTutorControllerService
      .getAllCourseLessonResources(this.courseLessonId as string)
      .subscribe({
        next: (response) => {
          console.log('Resources fetched successfully', response);
          this.resources = response.data;
          if (this.resources.length === 0) {
            this.snackboxService.showSnackbox(
              'No resources found for this lesson',
              'info'
            );
          }
        },
        error: (error) => {
          console.log('Failed to fetch resources', error);
          this.snackboxService.showSnackbox(
            `Failed to load resources${
              error.error.message ? ', ' + error.error.message : ''
            }`,
            'error'
          );
        },
      });
  }

  onViewResource(resourceId: string) {
    this.router.navigate([
      `/tutor/course/lessons/resources/details/${this.courseId}/${this.courseLessonId}/${resourceId}`,
    ]);
  }
}
