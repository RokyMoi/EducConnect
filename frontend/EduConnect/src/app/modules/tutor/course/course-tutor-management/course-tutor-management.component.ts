import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GetCourseManagementDashboardInfoResponse } from '../../../../models/course/course-tutor-controller/get-course-management-dashboard-info-response';
import formatFileSize from '../../../../helpers/format-file-size.helper';

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
    this.router.navigate(['/tutor/course/lessons/new/' + this.courseId]);
  }
}
