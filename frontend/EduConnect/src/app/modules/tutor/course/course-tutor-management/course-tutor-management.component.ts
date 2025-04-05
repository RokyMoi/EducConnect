import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GetCourseManagementDashboardInfoResponse } from '../../../../models/course/course-tutor-controller/get-course-management-dashboard-info-response';
import formatFileSize from '../../../../helpers/format-file-size.helper';
import { PublishedStatus } from '../../../../../enums/published-status.enum';

@Component({
  selector: 'app-course-management-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-tutor-management.component.html',
  styleUrl: './course-tutor-management.component.css',
})
export class CourseTutorManagementComponent implements OnInit {
  dashboardInfo: GetCourseManagementDashboardInfoResponse | null = null;

  courseId: string | null = null;
  constructor(
    private courseTutorControllerService: CourseTutorControllerService,
    private snackboxService: SnackboxService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
  }
  ngOnInit(): void {
    this.loadDashboardInfo();
  }

  loadDashboardInfo() {
    this.courseTutorControllerService
      .getCourseManagementDashboardInfo(this.courseId as string)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.dashboardInfo = response.data;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            'Failed to load dashboard info, please try again later',
            'error'
          );
        },
      });
  }

  goBack() {
    this.router.navigate(['/tutor/course']);
  }

  onViewBasics() {
    this.router.navigate(['tutor/course/details/' + this.courseId]);
  }

  onViewThumbnail() {
    this.router.navigate(['tutor/course/thumbnail/' + this.courseId]);
  }

  onViewTeachingResources() {
    this.router.navigate(['tutor/course/teaching-resources/' + this.courseId]);
  }

  getFileSize(fileSize: number) {
    return formatFileSize(fileSize);
  }

  onAddNewResource() {
    this.router.navigate([
      '/tutor/course/teaching-resources/new/' + this.courseId,
    ]);
  }

  onViewResource(courseTeachingResourceId: string) {
    this.router.navigate([
      '/tutor/course/teaching-resources/details',
      this.courseId,
      courseTeachingResourceId,
    ]);
  }

  onViewLessons() {
    this.router.navigate(['/tutor/course/lessons/' + this.courseId]);
  }
  getPublishedStatus(publishedStatus: number): string {
    return PublishedStatus[publishedStatus];
  }
  getLessonDisplayText(lesson: any): string {
    // Replace 'any' with the actual type of lesson
    const orderText = lesson.lessonSequenceOrder
      ? `Lesson ${lesson.lessonSequenceOrder}`
      : 'No Order Set';
    return `${lesson.title} - ${orderText} (${this.getPublishedStatus(
      lesson.publishedStatus
    )})`;
  }
  getLessonStatusColor(lesson: any): string {
    switch (lesson.publishedStatus) {
      case PublishedStatus.Published:
        return '#4CAF50'; // Green
      case PublishedStatus.Draft:
        return '#FFC107'; // Amber/Orange
      case PublishedStatus.Archived:
        return '#F44336'; // Red
      default:
        return 'grey'; // Default color if status is unknown
    }
  }
}
